import collections
import json
import re
from textwrap import indent

supported_prefixes = ['main', 'ed']
supported_instrs = ['nop', 'halt',
                    'ld', 'ex',
                    'jr', 'jp', 'call', 'ret', 'push', 'pop', 'djnz', 'rst',
                    'bit', 'set', 'res',
                    'inc', 'dec', 'add', 'adc', 'sub', 'sbc',
                    'or', 'and', 'xor', 'cpl',
                    'rlca', 'rla', 'rrca', 'rra',
                    'di', 'ei', 'im',
                    'exx',
                    'ldi', 'ldir', 'cpi', 'cpir', 'ini', 'inir', 'outi', 'otir',
                    'ldd', 'lddr', 'cpd', 'cpdr', 'ind', 'indr', 'outd', 'otdr']

Instruction = collections.namedtuple('Instruction', ['opcode',
                                                     'mnemonics',
                                                     'name',
                                                     'operands',
                                                     'description',
                                                     'size',
                                                     'cycles'])

prefix_template = '''{name} = new Instruction[] {{
{opcodes}
}};
'''

def prefix(data, prefix):
  instructions = [extract(instr) for instr in data[prefix] if instr['mnemonics'].split(' ')[0] in supported_instrs]

  code = []
  for opcode in range(0x100):
    try:
      instr = next(instr for instr in instructions if instr.opcode == opcode)
      code.append(instruction(instr))
    except StopIteration:
      code.append('  null')

  return prefix_template.format(name=prefix,
                                opcodes=',\n'.join(code))

def extract(instr):
  mnemonics = instr['mnemonics']
  parts = mnemonics.split(' ')
  name = parts[0]
  operands = parts[1].split(',') if len(parts) > 1 else []

  return Instruction(
    instr['opcode'],
    mnemonics,
    name,
    operands,
    instr['description'],
    instr['size'],
    instr['cycles']
  )

def instruction(instr):
  return '  new Instruction(cpu, 0x{op:02X}, "{m}", "{d}", {s}, {c}, Instructions.{n}({operands}))'.format(op=instr.opcode,
                                                                                                           m=instr.mnemonics,
                                                                                                           n=instr.name,
                                                                                                           s=instr.size,
                                                                                                           c=instr.cycles if isinstance(instr.cycles, int) else instr.cycles[0], # TODO conditionals,
                                                                                                           d=instr.description,
                                                                                                           operands=', '.join(['cpu'] + [operand(o, instr) for o in instr.operands]))

def operand(op, instr):

  # Registers
  if op in 'afbcdehlsp':
    return 'new Register{}(cpu, "{}")'.format(8 if len(op) == 1 else 16, op)

  if op in 'ir':
    return 'new Fixed(0)' # TODO

  # Immediate value
  if op in '**':
    return 'new Immediate{}(cpu)'.format(8 if len(op) == 1 else 16) # TODO handle position

  # Fixed value
  if op.endswith('h'):
    return 'new Fixed(0x{:02X})'.format(int(op[:-1], 16))

  # Fixed value
  if op in '0/1/2':
    return 'new Fixed({})'.format(int(op[0])) # TODO handle 1/2 and 0/1

  # Indirect value
  if op.startswith('(') and op.endswith(')'):
    if instr.name == 'ld' and len(instr.mnemonics) == 10 or instr.mnemonics == 'jp (hl)':
      return 'new Indirect16(cpu, {})'.format(operand(op[1:-1], instr))
    else:
      return 'new Indirect8(cpu, {})'.format(operand(op[1:-1], instr))

  # Alt register
  if op.startswith('af_'):
    return 'new Register16(cpu, "af" /* TODO alt! */)' # TODO

  # Flag condition
  if op.startswith('f_'):

    flag = {
      'c': 'carry',
      'z': 'zero',
      'p': 'parity',
      's': 'sign'
    }.get(op[-1])

    state = 'true' if len(op[2:]) == 1 else 'false'

    return 'new FlagCondition(cpu, "{}", {})'.format(flag, state)

  print('UNKNOWN {}'.format(op))
  return 'Unimplemented()'.format(op)


def inject(prefixes):
  path = '../z80/InstructionSet.cs'
  start_tag = '// GENERATED CODE STARTS HERE'
  end_tag = '// GENERATED CODE ENDS HERE'
  regex = re.compile(r'(?<={}\n).*?(?={})'.format(start_tag, end_tag), re.DOTALL)

  with open(path, 'r') as file:
    original = file.read()

  injected = regex.sub('{}\n        '.format(indent('\n'.join(prefixes), '  ' * 3)), original)

  with open(path, 'w') as file:
    file.write(injected)

if __name__ == '__main__':
  """
  This script reads the specifications of the Z80 opcodes from
  an external file and generates their C# implementations, which
  are then inserted into the main emulator code inside of
  CPU_Opcodes.cs.
  """

  # Get the specs
  with open('opcodes.json', 'r') as file:
    data = json.load(file)

  #
  prefixes = [prefix(data, p) for p in supported_prefixes]

  # Inject the generated code in the main project
  inject(prefixes)

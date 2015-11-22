import json, requests
from bs4 import BeautifulSoup


def fetch():
  r = requests.get('http://clrhome.org/table/')

  if not r.ok:
    print('Cannot fetch {})'.format(r.url))
    return None

  # remove newlines
  text = r.text.replace('\n', '')

  # Return the data as a BeautifulSoup object for easy querying
  return BeautifulSoup(text, 'html.parser')


def table_title(table):
  return 'main' if table['title'] == '' else table['title'].lower()


def parse_tables(page):
  return {table_title(table): parse_table(table)
          for table in page.find_all('table')}


def parse_table(table):
  print('Table {}'.format(table_title(table)))

  opcodes = []

  for td in table.find_all('td', axis=True):
    hi = int(td.parent.find('th').text, 16)  # row
    lo = td.parent.index(td) - 1  # column
    code = hi << 4 | lo

    specs = td['axis'].split('|')

    # Conditional instructions have different durations depending on how they
    # branch so the possible durations are stored in an array. Otherwise, the
    # duration is just stored as a single value.
    cycles = list(map(int, specs[2].split('/'))) if '/' in specs[2] else int(specs[2])

    opcodes.append({
      'opcode': code,
      'mnemonics': normalize(td.text).strip(),
      'size': int(specs[1]),
      'cycles': cycles,
      'flags': specs[0],
      'description': specs[3]
    })

    print('  {}: {}'.format(hex(code), td.text))

  return opcodes

def normalize(mnemonics):
  parts = mnemonics.split(' ')
  name = parts[0]
  operands = parts[1].split(',') if len(parts) > 1 else []
  return '{} {}'.format(name,
                        ','.join(normalize_operand(o, name) for o in operands))

def normalize_operand(operand, instr_name):

  # Flag condition
  if instr_name in ['jr', 'jp', 'ret', 'call'] and operand in ['c', 'nc', 'z', 'nz', 'po', 'pe', 'p', 'm']:
    operand = 'f_' + {
      'po': 'np',
      'pe': 'p',
      'p': 'ns',
      'm': 's'
    }.get(operand, operand)

  # Alt registers
  elif operand == 'af\'':
    operand = 'af_'

  return operand

if __name__ == '__main__':
  """
  This scripts fetches the contents of a webpage that contains
  nicely formatted data about the Z80 opcodes and outputs it
  to JSON.
  """

  page = fetch()

  if page is not None:
    opcodes = parse_tables(page)

    with open('opcodes.json', 'w') as output:
      json.dump(opcodes, output, indent=2)

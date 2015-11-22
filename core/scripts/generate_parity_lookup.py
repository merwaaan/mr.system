TEMPLATE = '''
bool[] parityLookup = new bool[] {{
  {}
}};'''


def parity(byte):
  """
  Return True if the number of bits that are set is even.
  """
  return str(bin(byte)).count('1') % 2 == 0


def to_csharp(bool):
  """
  Python to C# booleans.
  """
  return ['false', 'true'][bool]


if __name__ == '__main__':
  """
  This script generates a lookup table that gives the parity
  of every byte value, in the sense of the Z80 (number of bit
  that are set).
  """

  # Compute the parities of the possible byte values
  lookup = [parity(b) for b in range(0x100)]

  # Split into 16 chunks of 16 values and separate by line breaks
  lines = list(zip(*[map(to_csharp, lookup)] * 16))
  formatted = ',\n  '.join(', '.join(line) for line in lines)

  table = TEMPLATE.format(formatted)

  with open('parity_lookup.txt', 'w') as output:
    output.write(table)

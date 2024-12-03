import fileinput

a = []
b = []

for line in fileinput.input():
    parts = line.strip().split(' ')
    a.append(int(parts[0]))
    b.append(int(parts[-1]))

a.sort()
b.sort()

m = {}
for val in b:
    m[val] = m.get(val, 0) + 1

acc = sum(abs(x - y) for x, y in zip(a, b))
sim = sum((0 if x not in m else m[x]) * x for x in a)

print(str(acc))
print(str(sim))
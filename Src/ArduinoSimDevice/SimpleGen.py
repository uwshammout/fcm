from math import sin, cos, pi

offset = 1.0

for deg in range(0,360):
    rad = deg * pi / 180
    sine = sin(rad) + offset
    cosine = cos(rad) + offset
    print(f"{deg}, {rad} => {sine}, {cosine}")
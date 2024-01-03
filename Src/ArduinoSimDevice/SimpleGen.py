from math import sin, cos, pi

offset = 1.0
scale = 128

for deg in range(0,360):
    rad = deg * pi / 180
    
    sine = sin(rad)
    cosine = cos(rad)
    
    offset_sine = sine + offset
    offset_cosine = cosine + offset
    
    offset_scaled_sine = int(offset_sine * scale)
    offset_scaled_cosine = int(offset_cosine * scale)

    print(f"{deg}, {rad} => {offset_scaled_sine}, {offset_scaled_cosine}")
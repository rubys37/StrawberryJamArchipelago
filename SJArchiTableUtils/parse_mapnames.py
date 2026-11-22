with open("strawberries.txt") as file:
    berrydata = file.readlines()
with open("mapnames.txt") as file:
    mapdata = file.readlines()
berrydata = [i.split('=^.^=')[0].split("/")[2].lower() for i in berrydata]
mapdata ={i.split(':')[0].lower().strip(): i.split(':')[1].strip() for i in mapdata if i != "\n"}
[print(mapdata[i]) for i in berrydata]
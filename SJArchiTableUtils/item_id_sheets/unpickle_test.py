import pathlib
import pickle

path = pathlib.Path(__file__).parent.resolve() / "SJArchipelago_Tables - ItemIDs.pickle"
with open(path, "rb") as file:
    data = pickle.load(file)
print(data)
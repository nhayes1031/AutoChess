namespace Server.Game {
    public class CharacterList {
        private Character[] list;

        public int Size => list.Length;
        public Character[] List => list;

        public CharacterList(int size) {
            list = new Character[size];
        }

        public bool Remove(Character entry) {
            for (int i = 0; i < Size; i++) {
                if (list[i] != null) {
                    if (list[i].Name == entry.Name) {
                        list[i] = null;
                        return true;
                    }
                }
            }
            return false;
        }

        public void RemoveAt(int index) {
            list[index] = null;
        }

        public int RemoveAll(Character entry) {
            var count = 0;
            for (int i = 0; i < Size; i++) {
                if (list[i] != null) {
                    if (list[i].Name == entry.Name) {
                        list[i] = null;
                        count++;
                    }
                }
            }
            return count;
        }

        public void Clear() {
            for (int i = 0; i < Size; i++) {
                list[i] = null;
            }
        }

        public bool Add(Character character) {
            for (int i = 0; i < Size; i++) {
                if (list[i] == null) {
                    list[i] = character;
                    return true;
                }
            }
            return false;
        }

        public bool AddRange(Character[] characters) {
            if (characters.Length == Size) {
                for (int i = 0; i < Size; i++) {
                    list[i] = characters[i];
                }
                return true;
            }
            return false;
        }

        public bool Contains(Character character) {
            for (int i = 0; i < Size; i++) {
                if (list[i] != null) {
                    if (list[i].Name == character.Name) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

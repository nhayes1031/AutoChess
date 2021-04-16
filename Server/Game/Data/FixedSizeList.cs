namespace Server.Game {
    public class FixedSizeList<T> {
        private T[] list;

        public int Size => list.Length;
        public T[] List => list;

        public FixedSizeList(int size) {
            list = new T[size];
        }

        public bool Remove(T entry) {
            for (int i = 0; i < Size; i++) {
                if (!(list[i] is null) && list[i].Equals(entry)) {
                    list[i] = default;
                    return true;
                }
            }
            return false;
        }

        public void RemoveAt(int index) {
            list[index] = default;
        }

        public int RemoveAll(T entry) {
            var count = 0;
            for (int i = 0; i < Size; i++) {
                if (!(list[i] is null) && list[i].Equals(entry)) {
                    list[i] = default;
                    count++;
                }
            }
            return count;
        }

        public void Clear() {
            for (int i = 0; i < Size; i++) {
                list[i] = default;
            }
        }

        public bool Add(T entry) {
            for (int i = 0; i < Size; i++) {
                if (list[i] is null) {
                    list[i] = entry;
                    return true;
                }
            }
            return false;
        }

        public bool AddRange(T[] entries) {
            if (entries.Length == Size) {
                for (int i = 0; i < Size; i++) {
                    list[i] = entries[i];
                }
                return true;
            }
            return false;
        }

        public bool Contains(T entry) {
            for (int i = 0; i < Size; i++) {
                if (!(list[i] is null) && list[i].Equals(entry)) {
                    return true;
                }
            }
            return false;
        }
    }
}

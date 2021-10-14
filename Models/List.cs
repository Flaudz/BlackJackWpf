using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPrac.Models
{
    public class MyList<T> : INotifyPropertyChanged
    {
        private T[] array = new T[0];
        private int length;
        private int count;

        public T[] Array { get => array;
            set
            {
                if (array != value)
                {
                    array = value;
                    RaisePropertyChanged("Array");
                }
            }
        }
        public int Length { get => length; set => length = value; }
        public int Count { get => count; set => count = value; }

        public MyList()
        {

        }

        public void Add(T item)
        {
            this.ResizeTo(this.length + 1);
            this.Insert(item);
            this.Count++;
        }

        // Get List Method
        public T Get(int index)
        {
            return this.GetItem(index);
        }

        // Remove List Method
        public void Remove(int index)
        {
            for (int i = index; i < this.Count - 1; i++)
            {
                this.Swap(i, i + 1);
            }
            this.ResizeTo(this.Count - 1);
        }

        public bool Contains(T item)
        {
            if (this.Count != 0)
            {
                foreach (T Item in this)
                {
                    if (item.Equals(Item))
                        return true;
                }
            }
            return false;
        }

        public IEnumerator GetEnumerator()
        {
            int index = 0;
            T current = this.Get(index);

            while (index != this.length)
            {
                yield return current;
                current = this.Get(index);
                index++;
            }
        }

        protected T GetItem(int index)
        {
            return this.array[index];
        }

        protected void Insert(T data)
        {
            this.array[array.Length - 1] = data;
        }

        // ResizeTo List Method
        protected void ResizeTo(int newLength)
        {
            T[] newArr = new T[newLength];
            if (newLength < Count)
            {

                for (int i = 0; i < newLength; i++)
                {
                    newArr[i] = this.Array[i];
                }

                this.Count = newLength;

            }
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    newArr[i] = this.Array[i];
                }
            }
            this.Length = newLength;
            this.Array = newArr;
        }

        // ResizeTo List Method
        protected void ResizeStart(int newLength)
        {
            T[] newArr = new T[newLength];
            if (newLength < this.Count)
            {

                for (int i = 1; i < newLength + 1; i++)
                {
                    newArr[i - 1] = this.array[i];
                }

                this.count = newLength;

            }
            else
            {
                for (int i = 1; i < this.Count + 1; i++)
                {
                    newArr[i - 1] = this.array[i];
                }
            }
            this.length = newLength;
            this.array = newArr;
        }

        protected void Swap(int index1, int index2)
        {
            T temp = this.array[index1];
            this.array[index1] = this.array[index2];
            this.array[index2] = temp;
        }

        // Property Changed
        public event PropertyChangedEventHandler PropertyChanged;

        // This fuction is what informors the thing that it is changed
        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

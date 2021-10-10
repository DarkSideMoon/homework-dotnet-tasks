using System;
using System.Collections;
using System.Collections.Generic;

namespace Homework.Dotnet.Tasks.Services.Algorithms
{
    public class AvlTree<T> : ICollection<T>, IList<T> where T : IComparable<T>
    {
        public class TreeNode : ICollection<T>, IList<T>
        {
            public TreeNode(T value, AvlTree<T> avlTree)
            {
                Value = value;
                Level = 1;
                Count = 1;
                AvlTree = avlTree;
            }

            public AvlTree<T> AvlTree { get; }
            public T Value { get; private set; }
            public TreeNode Parent { get; private set; }
            public TreeNode LeftHand { get; private set; }
            public TreeNode RightHand { get; private set; }
            int Level { get; set; }
            public int Count { get; private set; }

            public void Add(T item)
            {
                var compare = item?.CompareTo(Value);
                if (compare < 0)
                    if (LeftHand == null)
                        ((LeftHand = new AvlTree<T>.TreeNode(item, AvlTree)).Parent = this).Reconstruct(true);
                    else LeftHand.Add(item);
                else if (RightHand == null)
                    ((RightHand = new AvlTree<T>.TreeNode(item, AvlTree)).Parent = this).Reconstruct(true);
                else RightHand.Add(item);
            }

            public void Clear()
            {
                LeftHand?.Clear();
                RightHand?.Clear();

                LeftHand = RightHand = null;
            }

            public bool Contains(T item)
            {
                var compare = item?.CompareTo(Value);
                return compare switch
                {
                    < 0 => LeftHand?.Contains(item) ?? false,
                    0 => true,
                    _ => RightHand != null && RightHand.Contains(item)
                };
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                if (LeftHand != null)
                {
                    LeftHand.CopyTo(array, arrayIndex);
                    arrayIndex += LeftHand.Count;
                }
                array[arrayIndex++] = Value;
                if (RightHand != null)
                    RightHand.CopyTo(array, arrayIndex);
            }

            public bool IsReadOnly => false;

            public bool Remove(T item)
            {
                var compare = item?.CompareTo(this.Value);
                if (compare == 0)
                {
                    if (this.LeftHand == null && this.RightHand == null)
                        if (this.Parent != null)
                        {
                            if (this.Parent.LeftHand == this) this.Parent.LeftHand = null;
                            else this.Parent.RightHand = null;
                            this.Parent.Reconstruct(true);
                        }
                        else this.AvlTree.RootNode = null;
                    else if (this.LeftHand == null || this.RightHand == null)
                    {
                        var child = this.LeftHand == null ? this.RightHand : this.LeftHand;
                        if (this.Parent != null)
                        {
                            if (this.Parent.LeftHand == this) this.Parent.LeftHand = child;
                            else this.Parent.RightHand = child;
                            (child.Parent = this.Parent).Reconstruct(true);
                        }
                        else (this.AvlTree.RootNode = child).Parent = null;
                    }
                    else
                    {
                        var replace = this.LeftHand;
                        while (replace.RightHand != null) replace = replace.RightHand;
                        var temp = this.Value;
                        this.Value = replace.Value;
                        replace.Value = temp;
                        return replace.Remove(replace.Value);
                    }
                    this.Parent = this.LeftHand = this.RightHand = null;
                    return true;
                }
                else if (compare < 0)
                    return this.LeftHand == null ? false : this.LeftHand.Remove(item);
                else
                    return this.RightHand == null ? false : this.RightHand.Remove(item);
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (this.LeftHand != null)
                    foreach (var item in this.LeftHand)
                        yield return item;
                yield return this.Value;
                if (this.RightHand != null)
                    foreach (var item in this.RightHand)
                        yield return item;
            }

            IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

            void Reconstruct(bool recursive)
            {
                this.Count = 1;

                int leftLevel = 0, rightLevel = 0;
                if (this.LeftHand != null)
                {
                    leftLevel = this.LeftHand.Level;
                    this.Count += this.LeftHand.Count;
                }
                if (this.RightHand != null)
                {
                    rightLevel = this.RightHand.Level;
                    this.Count += this.RightHand.Count;
                }

                if (leftLevel - rightLevel > 1)
                {
                    var leftLeft = this.LeftHand.LeftHand == null ? 0 : this.LeftHand.LeftHand.Level;
                    var leftRight = this.LeftHand.RightHand == null ? 0 : this.LeftHand.RightHand.Level;
                    if (leftLeft >= leftRight)
                    {
                        this.LeftHand.Elevate();
                        this.Reconstruct(true);
                    }
                    else
                    {
                        var pivot = this.LeftHand.RightHand;
                        pivot.Elevate(); pivot.Elevate();
                        pivot.LeftHand.Reconstruct(false);
                        pivot.RightHand.Reconstruct(true);
                    }
                }
                else if (rightLevel - leftLevel > 1)
                {
                    var rightRight = this.RightHand.RightHand == null ? 0 : this.RightHand.RightHand.Level;
                    var rightLeft = this.RightHand.LeftHand == null ? 0 : this.RightHand.LeftHand.Level;
                    if (rightRight >= rightLeft)
                    {
                        this.RightHand.Elevate();
                        this.Reconstruct(true);
                    }
                    else
                    {
                        var pivot = this.RightHand.LeftHand;
                        pivot.Elevate(); pivot.Elevate();
                        pivot.LeftHand.Reconstruct(false);
                        pivot.RightHand.Reconstruct(true);
                    }
                }
                else
                {
                    this.Level = Math.Max(leftLevel, rightLevel) + 1;
                    if (this.Parent != null && recursive)
                        this.Parent.Reconstruct(true);
                }
            }

            void Elevate()
            {
                var root = this.Parent;
                var parent = root.Parent;
                if ((this.Parent = parent) == null) this.AvlTree.RootNode = this;
                else
                {
                    if (parent.LeftHand == root) parent.LeftHand = this;
                    else parent.RightHand = this;
                }

                if (root.LeftHand == this)
                {
                    root.LeftHand = this.RightHand;
                    if (this.RightHand != null) this.RightHand.Parent = root;
                    this.RightHand = root;
                    root.Parent = this;
                }
                else
                {
                    root.RightHand = this.LeftHand;
                    if (this.LeftHand != null) this.LeftHand.Parent = root;
                    this.LeftHand = root;
                    root.Parent = this;
                }
            }

            public int IndexOf(T item)
            {
                var compare = item.CompareTo(this.Value);
                if (compare == 0)
                    if (this.LeftHand == null) return 0;
                    else
                    {
                        var temp = this.LeftHand.IndexOf(item);
                        return temp == -1 ? this.LeftHand.Count : temp;
                    }
                else if (compare < 0)
                    if (this.LeftHand == null) return -1;
                    else return this.LeftHand.IndexOf(item);
                else
                    if (this.RightHand == null) return -1;
                else return this.RightHand.IndexOf(item);
            }

            public void Insert(int index, T item) { throw new InvalidOperationException(); }

            public void RemoveAt(int index)
            {
                if (this.LeftHand != null)
                    if (index < this.LeftHand.Count)
                    {
                        this.LeftHand.RemoveAt(index);
                        return;
                    }
                    else index -= this.LeftHand.Count;
                if (index-- == 0)
                {
                    this.Remove(this.Value);
                    return;
                }
                if (this.RightHand != null)
                    if (index < this.RightHand.Count)
                    {
                        this.RightHand.RemoveAt(index);
                        return;
                    }
                throw new ArgumentOutOfRangeException("index");
            }

            public T this[int index]
            {
                get
                {
                    if (this.LeftHand != null)
                        if (index < this.LeftHand.Count) return this.LeftHand[index];
                        else index -= this.LeftHand.Count;
                    if (index-- == 0) return this.Value;
                    if (this.RightHand != null)
                        if (index < this.RightHand.Count) return this.RightHand[index];
                    throw new ArgumentOutOfRangeException("index");
                }
                set { throw new InvalidOperationException(); }
            }
        }

        public TreeNode RootNode { get; private set; }

        public void Add(T item)
        {
            if (this.RootNode == null) this.RootNode = new AvlTree<T>.TreeNode(item, this);
            else this.RootNode.Add(item);
        }

        public void Clear()
        {
            if (this.RootNode == null) return;
            this.RootNode.Clear();
            this.RootNode = null;
        }

        public bool Contains(T item) { return this.RootNode == null ? false : this.RootNode.Contains(item); }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex");
            if ((array.Length <= arrayIndex) || (this.RootNode != null && array.Length < arrayIndex + this.RootNode.Count))
                throw new ArgumentException();

            if (this.RootNode != null)
                this.RootNode.CopyTo(array, arrayIndex);
        }

        public int Count { get { return this.RootNode.Count; } }

        public bool IsReadOnly { get { return false; } }

        public bool Remove(T item) { return this.RootNode == null ? false : this.RootNode.Remove(item); }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.RootNode != null)
                foreach (var item in this.RootNode)
                    yield return item;
            else
                yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        public int IndexOf(T item) { return this.RootNode != null ? this.RootNode.IndexOf(item) : -1; }

        public void Insert(int index, T item) { throw new InvalidOperationException(); }

        public void RemoveAt(int index) { if (this.RootNode != null) this.RootNode.RemoveAt(index); }

        public T this[int index]
        {
            get
            {
                if (this.RootNode != null) return this.RootNode[index];
                else throw new ArgumentOutOfRangeException("index");
            }
            set { throw new InvalidOperationException(); }
        }
    }
}

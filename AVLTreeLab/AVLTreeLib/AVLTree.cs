using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*TODO:
 * Напишите класс AVLTree<TKey,TValue>, который содержит методы удаления элемента из дерева, 
 * поиска элемента в дереве, вставки элемента, балансировки.
 */
namespace AVLTreeLib
{
    internal class Node<TKey, TValue> where TKey : IComparable<TKey>
    {
        internal TKey Key { get; set; }
        internal TValue Value { get; set; }
        internal int Height { get; set; } // высота поддерева с корнем в данном узле
        internal Node<TKey,TValue> Left { get; set; } // ссылка на узел слева
        internal Node<TKey,TValue> Right { get; set; }

        internal Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Height = 1;
            Left = Right = null;
        }
    }

    public class AVLTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private Node<TKey,TValue> Root { get; set; }
        public int Count { get; private set; }

        public AVLTree() { Root = null;  Count = 0; }

        // O(1)
        private int Height(Node<TKey, TValue> node) 
        {
            // обертка над высотой поддерева
            return node == null ? 0 : node.Height;
        }

        /// <summary>
        /// Данный метод возвращает разницу высот левого и правого поддеревьев данного узла
        /// </summary>
        /// <param name="node"> Узел </param>
        /// <returns> Разница высот поддеревьев </returns>
        /// O(1)
        private int Discrepancy(Node<TKey,TValue> node)
        {
            return Height(node.Right) - Height(node.Left);
        }

        /// <summary>
        /// Данный метод корректирует высоту поддерева данного узла
        /// </summary>
        /// <param name="node"></param>
        /// O(1)
        private void FixHeight(Node<TKey,TValue> node)
        {
            if (node != null)
                node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;
        }

        /*
         В процессе добавления или удаления узлов в АВЛ-дереве возможно возникновение ситуации, 
         когда balance factor некоторых узлов оказывается равными 2 или -2, 
         т.е. возникает расбалансировка поддерева.*/

        /// <summary>
        /// Данный метод выполняет правый поворот вокруг данного узла
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        /// O(1)
        private Node<TKey, TValue> RotateRight(Node<TKey,TValue> node)
        {
            Node<TKey,TValue> leftNode = node.Left;
            node.Left = leftNode.Right;
            leftNode.Right = node;
            FixHeight(node);
            FixHeight(leftNode);

            return leftNode;
        }

        // O(1)
        private Node<TKey, TValue> RotateLeft(Node<TKey, TValue> node)
        {
            Node<TKey, TValue> rightNode = node.Right;
            node.Right = rightNode.Left;
            rightNode.Left = node;
            FixHeight(node);
            FixHeight(rightNode);

            return rightNode;
        }

        /// <summary>
        /// Данный метод выполняет балансировку в данном узле
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        /// O(1)
        private Node<TKey, TValue> Balance(Node<TKey, TValue> node)
        {
            // Метод сводится к проверке разницы высот между поддеревьями и выбором поворотов
            FixHeight(node);

            // если высота правого поддерева больше левого на 2, то ...
            if (Discrepancy(node) == 2)
            {
                // ... (обращаемся к правому поддереву текущего узла)
                // если высота левого поддерева больше правого поддерева, то ...
                if (Discrepancy(node.Right) < 0)
                    // выполним правый поворот вокруг правой вершины текущего узла
                    node.Right = RotateRight(node.Right);

                // выполняем левый поворот вокруг текущей вершины 
                return RotateLeft(node);
            }

            // если высота левого поддерева больше правого на 2, то ...
            if (Discrepancy(node) == -2)
            {
                // ... (обращаемся к левому поддереву текущего узла)
                // если высота правого поддерева больше левого поддерева, то ...
                if (Discrepancy(node.Left) > 0)
                    // выполним левый поворот вокруг левой вершины текущего узла
                    node.Left = RotateLeft(node.Left);

                return RotateRight(node);
            }

            return node;
        }

        /// <summary>
        /// Рекурсивный метод добавления нового узла
        /// </summary>
        /// <param name="node"> Узел, с которого начинается вставка</param>
        /// <param name="key"> Ключ </param>
        /// <param name="value"> Значение </param>
        /// <returns></returns>
        /// O(log N)
        private Node<TKey, TValue> Add(Node<TKey, TValue> node, TKey key, TValue value)
        {
            // рекурсивно спускаемся по дереву, пока не найдется место для вставки узла
            if (node == null) return new Node<TKey, TValue> (key, value);

            if (key.CompareTo(node.Key) < 0) node.Left = Add(node.Left, key, value);
            else node.Right = Add(node.Right, key, value);

            // возвращаясь из рекурсии, проводим балансировку
            return Balance(node);
        }

        public void Insert(TKey key, TValue value)
        {
            Root = Add(Root, key, value);
            Count++;
        }

        /// <summary>
        /// Данный метод ищет узел с минимальный ключем в поддереве данного узла
        /// </summary>
        /// <param name="node"> Узел </param>
        /// <returns></returns>
        private Node<TKey, TValue> FindMin(Node<TKey, TValue> node)
        {
            return node.Left != null ? FindMin(node.Left) : node;
        }

        // Данный метод удаляет узел с минимальным ключом
        private Node<TKey, TValue> RemoveMin(Node<TKey, TValue> node)
        {
            // у минимального элемента справа либо подвешен единственный узел, либо там пусто. 
            // В обоих случаях надо просто вернуть указатель на правый узел и ...
            if (node.Left == null) return node.Right;

            node.Left = RemoveMin(node.Left);

            // ...  по пути назад (при возвращении из рекурсии) выполнить балансировку.
            return Balance(node);
        }

        //O(log N)
        private Node<TKey, TValue> Delete(Node<TKey, TValue> node, TKey key)
        {
            if (node == null) return null;

            // ищем узел с заданным ключом
            if (key.CompareTo(node.Key) < 0)
                node.Left = Delete(node.Left, key);
            else if (key.CompareTo(node.Key) > 0)
                node.Right = Delete(node.Right, key);
            // если нашли, то
            else
            {
                // запоминаем левое и правое поддеревья
                Node<TKey, TValue> q = node.Left;
                Node<TKey, TValue> r = node.Right;
                //node = null;

                // если правое поддерево пустое, то...
                if (r == null)
                    // слева у этого узла может быть только один единственный дочерний узел (дерево высоты 1), либо узел вообще лист, тогда...
                    // возвращаем указатель на левое поддерево
                    return q;

                // если правое поддерево непустое, то находим там узел с минимальным ключом
                Node<TKey, TValue> min = FindMin(r);
                min.Right = RemoveMin(r); // подвешиваем справа то, что осталось после удаления минимального в правом поддереве
                min.Left = q; // подвешиваем левое поддерево

                return Balance(min);
            }

            return Balance(node);
        }

        public void Remove(TKey key)
        {
            Root = Delete(Root, key);
            Count--;
        }

        /// <summary>
        /// Данный метод показывает существует ли узел с заданным ключом
        /// </summary>
        /// <param name="key"> Ключ </param>
        /// <returns> True, если содержится, иначе - False </returns>
        /// O (log N)
        public bool Find(TKey key)
        {
            Node<TKey, TValue> node = Root;

            if (node == null) return false;

            while (node != null)
            {
                if (key.CompareTo(node.Key) < 0) node = node.Left;
                else if (key.CompareTo(node.Key) > 0) node = node.Right;
                else
                    return true;
            }
            return false;
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> DoInorderTraversal()
        {
            Stack<Node<TKey, TValue>> stack = new Stack<Node<TKey, TValue>>(Count);
            var current = Root;
            while (current != null || stack.Count > 0)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;

                }

                current = stack.Pop();
                //Console.WriteLine(current.Key);
                yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
                current = current.Right;

            }

        }
    }
}

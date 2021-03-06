using System;
using System.Collections.Generic;
using System.Text;

namespace Yanesdk.Ytl
{
	/// <summary>
	/// Removeの拡張を行なってあるLinkedList
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class LinkedListEx<T> : LinkedList<T>
	{
		/// <summary>
		/// 要素を削除
		/// </summary>
		/// <param name="t"></param>
		public new void Remove(T value)
		{
			base.Remove(value);
		}

		/// <summary>
		/// Removeで用いるdelegate。引数tが、削除したい条件と
		/// 合致するなら、trueを返す。そうすれば、その要素は削除される。
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public delegate bool RemoveDelegate(T t);

		/// <summary>
		/// 条件に合致した要素を削除
		/// </summary>
		/// <param name="t"></param>
		public void Remove(RemoveDelegate rd)
		{
			LinkedListNode<T> node = this.First;
			while ( node != null )
			{
				LinkedListNode<T> next = node.Next;
				if ( rd(node.Value) )
					base.Remove(node);
				node = next;
			}
		}
	}

	/// <summary>
	/// LinkedListExの非同期対応版
	/// </summary>
	/// <remarks>
	/// ただしすべてのメソッドを用意してあるわけではない。
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	public class SynchronizedLinkedList<T> : LinkedListEx<T>
	{
		/// <summary>
		/// [async]要素を先頭に追加
		/// </summary>
		/// <param name="t"></param>
		public new void AddFirst(T t)
		{
			lock ( SyncObject )
				base.AddFirst(t);
		}

		/// <summary>
		/// [async]要素をひとつ末尾に追加
		/// </summary>
		/// <param name="t"></param>
		public new void AddLast(T t)
		{
			lock ( SyncObject )
				base.AddLast(t);
		}

		/// <summary>
		/// [async]要素をクリア
		/// </summary>
		/// <param name="t"></param>
		public new void Clear()
		{
			lock ( SyncObject )
				base.Clear();
		}

		/// <summary>
		/// [async]要素を削除
		/// </summary>
		/// <param name="t"></param>
		public new void Remove(T value)
		{
			lock ( SyncObject )
				base.Remove(value);
		}


		/// <summary>
		/// [async]条件に合致した要素を削除
		/// </summary>
		/// <param name="t"></param>
		public new void Remove(RemoveDelegate rd)
		{
			lock ( SyncObject )
				base.Remove(rd);
		}


		/// <summary>
		/// 同期用オブジェクト
		/// </summary>
		/// <remarks>
		/// 上記のメンバだけでは足りないときは、この同期用オブジェクトを
		/// lockして、baseメンバを呼び出してください。
		/// </remarks>
		public object SyncObject = new object();
	}
}

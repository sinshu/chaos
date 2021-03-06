using System;
using System.Collections.Generic;
using System.Text;

namespace Yanesdk.Ytl
{
	/// <summary>
	/// スレッド間で共有するオブジェクト
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ThreadSharedObject<T>
	{
		/// <summary>
		/// スレッド間で共有するオブジェクト
		/// </summary>
		/// <remarks>
		/// OnRead/OnWriteに先行して、このメンバを設定すること。
		/// このメンバを読み出して、勝手に読み込んだり書き込んだりすべきではない。
		/// (するならば、SyncObjectをlockして行なうべし)
		/// </remarks>
		public T SharedObject
		{
			get { return sharedObject; }
			set { sharedObject = value; }
		}
		private T sharedObject;


		/// <summary>
		/// OnReadで用いるdelegate
		/// </summary>
		/// <param name="data"></param>
		public delegate void OnReadDelegate(T data);

		/// <summary>
		/// OnWriteで用いるdelegate
		/// </summary>
		/// <param name="data"></param>
		public delegate void OnWriteDelegate(T data);

		/// <summary>
		/// [async]共有オブジェクトのread
		/// </summary>
		/// <param name="doWork"></param>
		public void OnRead(OnReadDelegate doWork)
		{
			lock ( this.SyncObject )
			{
				doWork(this.sharedObject);
			}
		}

		/// <summary>
		/// [async]共有オブジェクトへのwrite
		/// </summary>
		public void OnWrite(OnWriteDelegate doWork)
		{
			lock ( this.SyncObject )
			{
				doWork(this.sharedObject);
				isDirty = true;
			}
		}

		/// <summary>
		/// Writeしたときに付く、汚しフラグ。
		/// </summary>
		public bool IsDirty
		{
			get { return isDirty; }
		}
		private bool isDirty;

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

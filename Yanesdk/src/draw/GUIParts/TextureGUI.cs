using System;
using System.Collections.Generic;
using System.Text;

using Yanesdk.Ytl;
using Yanesdk.Draw;
using Yanesdk.Input;
using Yanesdk.Sound;

namespace Yanesdk.GUIParts
{
	/// <summary>
	/// ITextureクラスで構成された画面部品の基底クラス
	/// </summary>
	/// <remarks>
	/// 自作のGUIPartsを作りたいときは、
	/// TextureButtonクラスを参考にしてください。
	/// </remarks>
	public interface ITextureGUI
	{
		/// <summary>
		/// 初期化のときに呼び出される
		/// </summary>
		/// <param name="cc"></param>
		void OnInit(ControlContext cc);

		/// <summary>
		/// 部品の描画を行なう
		/// </summary>
		/// <param name="context"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		void OnPaint(IScreen scr, ControlContext cc, int x, int y);

		/// <summary>
		/// このpartsは有効なのか？
		/// </summary>
		/// <remarks>
		/// このフラグがfalseのときは、OnPaintで何も描画せずに返るコードを書く。
		/// </remarks>
		bool Visible
		{
			get;
			set;
		}
	}

	/// <summary>
	/// GUIPartsが実装のときに必要とする構造体
	/// </summary>
	public class ControlContext
	{
		/// <summary>
		/// ここで渡しているMouseInputは、毎フレーム、Updateを呼び出し、
		/// 情報が更新されているものとする。
		/// </summary>
		public MouseInput MouseInput;

		/// <summary>
		/// テクスチャの読み込み用
		/// </summary>
		public SmartTextureLoader SmartTextureLoader;

		/// <summary>
		/// ボタンhoverのときなどにサウンドを再生するなら、これを用いる。
		/// </summary>
		/// <remarks>
		/// 用いないならnullのままで構わない。
		/// </remarks>
		public SmartSoundLoader SmartSoundLoader;

		/// <summary>
		/// 描画コンテキスト
		/// </summary>
		//public DrawContext DrawContext;

		/// <summary>
		/// ユーザーが自作のITextureGUI派生クラスを作ろうとするとき、
		/// 何かパラメータを渡す必要があれば、これを使うと良い。
		/// </summary>
		public Object Param;
	}

	/// <summary>
	/// ITextureクラスで構成された部品を配置するためのコントロールクラス
	/// (.NET FrameworkのControlクラスみたいなもの)
	/// 
	/// 使い方。
	///		1)TextureGUIControlを生成。
	/// 
	///		2)ITextureGUI派生クラスを生成して、
	///			TextureGUIControl.Controlで“包んで”TextureGUIControlに設定。
	/// 
	///		3)TextureGUIControlのInitで初期化。TextureGUIControl.OnDrawで描画。
	/// 
	/// </summary>
	/// <remarks>
	/// Controlを集約するControlが存在するような作りにすることも考えたが、
	/// こっちの構造でも良いと思った。
	/// 
	/// ITextureGUI派生クラスなので、ITextureGUIのフリをして、他のControlに埋め込むことも可能。
	/// </remarks>
	public class TextureGUIControl : ITextureGUI
	{
		/// <summary>
		/// ひとつのコントロールを表す構造体。
		/// </summary>
		public class Control
		{
			/// <summary>
			/// 描画したいPartsと表示したい座標を設定する
			/// </summary>
			/// <param name="parts"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			public Control(ITextureGUI parts, int x, int y)
			{
				this.Parts = parts;
				this.X = x;
				this.Y = y;
			}

			/// <summary>
			/// 描画したいPartsと表示したい座標とコントロールの名前を設定する。
			/// ここで設定した名前は、コントロールを取り除くときに必要となる。
			/// </summary>
			/// <param name="parts"></param>
			/// <param name="x"></param>
			/// <param name="y"></param>
			/// <param name="name"></param>
			public Control(ITextureGUI parts, int x, int y, string name)
			{
				this.Parts = parts;
				this.X = x;
				this.Y = y;
				this.name = name;
			}

			/// <summary>
			/// 部品
			/// </summary>
			public ITextureGUI Parts;

			/// <summary>
			/// 配置する座標
			/// </summary>
			public int X, Y;

			/// <summary>
			/// 配置する座標の設定
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			public void SetXY(int x, int y)
			{
				X = x; Y = y;
			}

			/// <summary>
			/// 部品名(部品を消すときは、これを手がかりに消す)
			/// </summary>
			public string Name
			{
				get { return name; }
				set { name = value; }
			}
			private string name;
		}

		/// <summary>
		/// 保持しているControls
		/// </summary>
		public LinkedListEx<Control> Controls
		{
			get { return controls; }
			set { controls = value; }
		}
	
		private LinkedListEx<Control> controls = new LinkedListEx<Control>();

		/// <summary>
		/// Controlの描画を行なう。
		/// コントロールは事前に配置し、かつ、描画コンテキストは事前に設定しておくこと。
		/// </summary>
		public void OnPaint(IScreen scr)
		{
			if ( visible )
				foreach ( Control c in Controls )
					c.Parts.OnPaint(scr,controlContext,c.X, c.Y);
		}

		/// <summary>
		/// Controlの描画を行なう。
		/// コントロールは事前に配置し、かつ、描画コンテキストは事前に設定しておくこと。
		/// 
		/// 座標を指定できる。
		/// </summary>
		public void OnPaint(IScreen scr , int x , int y)
		{
			if ( visible )
				foreach ( Control c in Controls )
					c.Parts.OnPaint(scr , controlContext , c.X + x, c.Y + y);
		}

		/// <summary>
		/// 初期化のときに呼び出される
		/// </summary>
		/// <remarks>
		/// ITextureとして振舞うために必要。
		/// </remarks>
		/// <param name="cc"></param>
		public void OnInit(ControlContext cc)
		{
			this.controlContext = cc;
		}

		/// <summary>
		/// 部品の描画を行なう
		/// </summary>
		/// <remarks>
		/// このクラス自体がITextureとして振舞うために必要。
		/// </remarks>
		/// <param name="context"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void OnPaint(IScreen scr , ControlContext cc , int x , int y)
		{
			if (visible)
				foreach ( Control c in Controls )
				{
					c.Parts.OnPaint(scr , cc , c.X + x ,c.Y + y);
				}
		}

		/// <summary>
		/// このpartsは有効なのか？
		/// defaultではtrue
		/// </summary>
		/// <remarks>
		/// このフラグがfalseのときは、OnPaintで何も描画せずに返るコードを書く。
		/// </remarks>
		public bool Visible
		{
			get { return visible; } 
			set { visible = value; }
		}
		private bool visible = true;
		
		/// <summary>
		/// このControl本体に、GUIPartsを追加。
		/// 追加する瞬間に、GUIPartsは初期化(OnInitの呼び出し)がなされる。
		/// </summary>
		/// <remarks>
		/// このメソッドでOnInitが呼び出される。そのときにControlContextがパラメータとして渡される。
		/// よって、ControlContextはこのメソッド呼び出しより先行して初期化されていなければならない。
		/// 
		/// たとえば、TextureGUIControlにAddControlでTextureGUIControl(これはITexture派生クラスなので
		/// AddControlできる)する場合、次の順番で行なう必要がある。
		///		TextureGUIControl master_control;
		///		TextureGUIControl sub_control;
		///		TextureButton button;
		/// 
		///		master_control.AddControl(sub_control,10,20);
		///		sub_control.AddControl(button,30,30);
		///		※　ちなみに、この場合buttonは、(10,20)+(30,30) = (40,50)に描画される。
		///		この順番を逆にして、
		///		sub_control.AddControl(button,30,30);
		///		master_control.AddControl(sub_control,10,20);
		///		こうした場合、sub_controlのControlContextが初期化されていないので、
		///		sub_control.AddControl(button,30,30)で、buttonのOnInitを呼び出すときに
		///		OnInit(null)で呼び出してしまいマズイ。
		/// 
		/// </remarks>
		/// <param name="c"></param>
		public void AddControl(Control c)
		{
			c.Parts.OnInit(this.controlContext);
			Controls.AddLast(c);
		}

		/// <summary>
		/// ITextureGUIを直接登録する機能
		/// </summary>
		/// <remarks>
		/// このメソッドでOnInitが呼び出される。そのときにControlContextがパラメータとして渡される。
		/// よって、ControlContextはこのメソッド呼び出しより先行して初期化されていなければならない。
		/// 
		/// たとえば、TextureGUIControlにAddControlでTextureGUIControl(これはITexture派生クラスなので
		/// AddControlできる)する場合、次の順番で行なう必要がある。
		///		TextureGUIControl master_control;
		///		TextureGUIControl sub_control;
		///		TextureButton button;
		/// 
		///		master_control.AddControl(sub_control,10,20);
		///		sub_control.AddControl(button,30,30);
		///		※　ちなみに、この場合buttonは、(10,20)+(30,30) = (40,50)に描画される。
		///		この順番を逆にして、
		///		sub_control.AddControl(button,30,30);
		///		master_control.AddControl(sub_control,10,20);
		///		こうした場合、sub_controlのControlContextが初期化されていないので、
		///		sub_control.AddControl(button,30,30)で、buttonのOnInitを呼び出すときに
		///		OnInit(null)で呼び出してしまいマズイ。
		/// 
		/// </remarks>
		/// <param name="gui"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void AddControl(ITextureGUI gui , int x , int y)
		{
			AddControl(new Control(gui , x , y));
		}

		/// <summary>
		/// ITextureGUIを直接登録する機能
		/// </summary>
		/// <remarks>
		/// このメソッドでOnInitが呼び出される。そのときにControlContextがパラメータとして渡される。
		/// よって、ControlContextはこのメソッド呼び出しより先行して初期化されていなければならない。
		/// 
		/// たとえば、TextureGUIControlにAddControlでTextureGUIControl(これはITexture派生クラスなので
		/// AddControlできる)する場合、次の順番で行なう必要がある。
		///		TextureGUIControl master_control;
		///		TextureGUIControl sub_control;
		///		TextureButton button;
		/// 
		///		master_control.AddControl(sub_control,10,20);
		///		sub_control.AddControl(button,30,30);
		///		※　ちなみに、この場合buttonは、(10,20)+(30,30) = (40,50)に描画される。
		///		この順番を逆にして、
		///		sub_control.AddControl(button,30,30);
		///		master_control.AddControl(sub_control,10,20);
		///		こうした場合、sub_controlのControlContextが初期化されていないので、
		///		sub_control.AddControl(button,30,30)で、buttonのOnInitを呼び出すときに
		///		OnInit(null)で呼び出してしまいマズイ。
		/// 
		/// </remarks>
		/// <param name="gui"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="name">
		/// Controlの名前。Control(gui,x,y,name)を直接呼び出している。
		/// </param>
		public void AddControl(ITextureGUI gui , int x , int y,string name)
		{
			AddControl(new Control(gui , x , y,name));
		}

		/// <summary>
		/// コントロール名を指定してコントロールを削除
		/// </summary>
		/// <param name="name"></param>
		public void RemoveControl(string name)
		{
			controls.Remove(
				delegate(Control c) { return c.Name == name; }
			);
		}

		/// <summary>
		/// guiのinstanceを指定してコントロールを削除
		/// </summary>
		/// <param name="name"></param>
		public void RemoveControl(ITextureGUI gui)
		{
			controls.Remove(
				delegate(Control c) { return c.Parts == gui; }
			);
		}

		/// <summary>
		/// 名前の合致するコントロールを返す。
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Control GetControl(string name)
		{
			foreach (Control c in controls)
			{
				if (c.Name == name)
					return c;
			}
			return null;
		}

		/// <summary>
		/// 破棄する。破棄するときに、保持しているコントロールに関しては
		/// Disposable interfaceを持つならば、それらのDisposeを呼び出す。
		/// </summary>
		public void Dispose()
		{
			foreach (Control c in Controls)
			{
				IDisposable d = c.Parts as IDisposable;
				if (d != null) d.Dispose();
			}
		}

		/// <summary>
		/// ControlContextの各メンバを、設定したものを渡してください。
		/// </summary>
		/// <param name="context"></param>
		public ControlContext ControlContext
		{
			get { return controlContext; }
			set { controlContext = value; }
		}

		private ControlContext controlContext;

	}
}

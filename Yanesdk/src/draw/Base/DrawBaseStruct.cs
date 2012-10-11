using System;

namespace Yanesdk.Draw
{
	/// <summary>
	/// 矩形を表している構造体
	/// </summary>
	/// <remarks>
	/// 矩形(Left,Top)-(Right,Bottom)であるが、(Right,Bottom)は含まない。
	/// 
	/// また、Left >= Right ,  Bottom  >= Top であることが前提条件。
	/// ただし特殊な用途で使う場合、↑の限りではない。
	/// </remarks>
	public class Rect {
		/// <summary>
		/// 左。
		/// </summary>
		public float Left;
		/// <summary>
		/// 上。
		/// </summary>
		public float Top;
		/// <summary>
		/// 右。
		/// </summary>
		public float Right;
		/// <summary>
		/// 下。
		/// </summary>
		public float Bottom;

		public Rect()
		{
			Left = Right = Top = Bottom = 0;
		}


		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="left_"></param>
		/// <param name="top_"></param>
		/// <param name="right_"></param>
		/// <param name="bottom_"></param>
		public Rect(float left_, float top_, float right_, float bottom_) {
			Left = left_;
			Top = top_;
			Right = right_;
			Bottom = bottom_;
		}

		/// <summary>
		/// 各データを設定する。
		/// </summary>
		/// <param name="left_"></param>
		/// <param name="top_"></param>
		/// <param name="right_"></param>
		/// <param name="bottom_"></param>
		public void SetRect(float left_,float top_,float right_,float bottom_)	{
			Left = left_;
			Top  = top_;
			Right = right_;
			Bottom = bottom_;
		}

		/// <summary>
		/// 幅を取得する
		/// </summary>
		/// <remarks>
		/// Right - Left をしているだけ。符号がマイナスになる場合もありうる
		/// </remarks>
		/// <returns></returns>
		public float Width {
			get { return Right - Left; }
		}

		/// <summary>
		/// 高さを取得する
		/// </summary>
		/// <remarks>
		/// Bottom - Top をしているだけ。符号がマイナスになる場合もありうる。
		/// </remarks>
		public float Height
		{
			get { return Bottom - Top; }
		}

		/// <summary>
		/// 指定の座標が、この矩形に含まれるのかを判定する。
		/// Left >= Right ,  Bottom  >= Top であることが前提条件。
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsIn(float x , float y)
		{
			return ( Left <= x && x < Right && Top <= y && y < Bottom );
		}
	}

	
	/// <summary>
	/// サイズを表している構造体。
	/// </summary>
	public class Size {
		/// <summary>
		/// 幅。
		/// </summary>
		public float Cx;

		/// <summary>
		/// 高さ。
		/// </summary>
		public float Cy;

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="cx_"></param>
		/// <param name="cy_"></param>
		public Size(float cx_, float cy_) {
			Cx = cx_;
			Cy = cy_;
		}

		/// <summary>
		/// 各データを設定する。
		/// </summary>
		/// <param name="Cx"></param>
		/// <param name="Cy"></param>
		public void SetSize(float cx,float cy){
			this.Cx = cx; this.Cy = cy;
		}
	}

	/// <summary>
	/// 点を表している構造体
	/// </summary>
	public struct Point {
		/// <summary>
		/// x座標。
		/// </summary>
		public float X;
		/// <summary>
		/// y座標。
		/// </summary>
		public float Y;

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="x_"></param>
		/// <param name="y_"></param>
		public Point(float x_, float y_) {
			X = x_;
			Y = y_;
		}

		/// <summary>
		/// 各データを設定する。
		/// </summary>
		/// <param name="x_"></param>
		/// <param name="y_"></param>
		public void SetPoint(float x_,float y_) {
			X = x_; Y = y_;
		}
	}

	/// <summary>
	/// 色を表す構造体
	/// このクラス名の末尾のubはunsigned byteの略。
	///	OpenGLのほうでこのように名前をつける習慣がある)
	/// </summary>
	public struct Color4ub {
		/// <summary>
		/// 色情報
		/// </summary>
		/// <remarks>
		/// r,g,bは赤,緑,青の光の三原色。0～255までの値で。
		/// aはαチャンネル。0ならば完全な透明(配置したときに背景が透けて見える)
		/// 255なら不透明。0～255までの値で。
		/// </remarks>
		public byte R,G,B,A; 

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="r_"></param>
		/// <param name="g_"></param>
		/// <param name="b_"></param>
		/// <param name="a_"></param>
		public Color4ub(byte r_, byte g_, byte b_, byte a_) {
			R = r_;
			G = g_;
			B = b_;
			A = a_;
		}

		/// <summary>
		/// 色のリセット。r=g=b=a=255に。
		/// </summary>
		public void ResetColor() { R=G=B=A=255;}

		/// <summary>
		/// 色のセット r,g,bは0～255。aは自動的に255になる。
		/// </summary>
		/// <param name="r_"></param>
		/// <param name="g_"></param>
		/// <param name="b_"></param>
		public void SetColor(int r_,int g_,int b_) {
			R = (byte)r_; G = (byte)g_; B = (byte)b_; A = 255;
		}

		/// <summary>
		/// 色のセット r,g,b,aは0～255
		/// </summary>
		/// <param name="r_"></param>
		/// <param name="g_"></param>
		/// <param name="b_"></param>
		/// <param name="a_"></param>
		public void SetColor(int r_,int g_,int b_,int a_) {
			R = (byte)r_; G = (byte)g_; B = (byte)b_; A = (byte)a_;
		}

		/// <summary>
		/// 色の掛け算
		/// </summary>
		/// <remarks>
		/// r,g,b,aに関して、
		///   r = r1 * r2 / 255
		///   g = g1 * g2 / 255
		///   b = b1 * b2 / 255
		///   a = a1 * a2 / 255
		/// を行なう。
		/// </remarks>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns></returns>
		public static Color4ub operator*(Color4ub lhs, Color4ub rhs){
			Color4ub color_;
			//		color_.r = r*color.r/255;
			//	255で割るのは255足して256で割ればほぼ同じだ
			/*
				詳しいことは、やね本2を見ること！
			*/
			color_.R = (byte)((lhs.R+255)*rhs.R >> 8);
			color_.G = (byte)((lhs.G+255)*rhs.G >> 8);
			color_.B = (byte)((lhs.B+255)*rhs.B >> 8);
			color_.A = (byte)((lhs.A+255)*rhs.A >> 8);

			return color_;
		}
	}
}

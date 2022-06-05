// by Freya Holmér (https://github.com/FreyaHolmer/Mathfs)
// Do not manually edit - this file is generated by MathfsCodegen.cs

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Freya {

	/// <summary>An optimized uniform 2D Quadratic bézier segment, with 3 control points</summary>
	[Serializable] public struct BezierQuad2D : IParamCubicSplineSegment2D {

		const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;

		/// <summary>Creates a uniform 2D Quadratic bézier segment, from 3 control points</summary>
		/// <param name="p0">The starting point of the curve</param>
		/// <param name="p1">The middle control point of the curve, sometimes called a tangent point</param>
		/// <param name="p2">The end point of the curve</param>
		public BezierQuad2D( Vector2 p0, Vector2 p1, Vector2 p2 ) {
			( this.p0, this.p1, this.p2 ) = ( p0, p1, p2 );
			validCoefficients = false;
			curve = default;
		}

		Polynomial2D curve;
		public Polynomial2D Curve {
			get {
				ReadyCoefficients();
				return curve;
			}
		}
		#region Control Points

		[SerializeField] Vector2 p0, p1, p2;

		/// <summary>The starting point of the curve</summary>
		public Vector2 P0 {
			[MethodImpl( INLINE )] get => p0;
			[MethodImpl( INLINE )] set => _ = ( p0 = value, validCoefficients = false );
		}

		/// <summary>The middle control point of the curve, sometimes called a tangent point</summary>
		public Vector2 P1 {
			[MethodImpl( INLINE )] get => p1;
			[MethodImpl( INLINE )] set => _ = ( p1 = value, validCoefficients = false );
		}

		/// <summary>The end point of the curve</summary>
		public Vector2 P2 {
			[MethodImpl( INLINE )] get => p2;
			[MethodImpl( INLINE )] set => _ = ( p2 = value, validCoefficients = false );
		}

		/// <summary>Get or set a control point position by index. Valid indices from 0 to 2</summary>
		public Vector2 this[ int i ] {
			get =>
				i switch {
					0 => P0,
					1 => P1,
					2 => P2,
					_ => throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 2 range, and I think {i} is outside that range you know" )
				};
			set {
				switch( i ) {
					case 0:
						P0 = value;
						break;
					case 1:
						P1 = value;
						break;
					case 2:
						P2 = value;
						break;
					default: throw new ArgumentOutOfRangeException( nameof(i), $"Index has to be in the 0 to 2 range, and I think {i} is outside that range you know" );
				}
			}
		}

		#endregion
		[NonSerialized] bool validCoefficients;

		[MethodImpl( INLINE )] void ReadyCoefficients() {
			if( validCoefficients )
				return; // no need to update
			validCoefficients = true;
			curve = CharMatrix.quadraticBezier.GetCurve( p0, p1, p2 );
		}
		public static bool operator ==( BezierQuad2D a, BezierQuad2D b ) => a.P0 == b.P0 && a.P1 == b.P1 && a.P2 == b.P2;
		public static bool operator !=( BezierQuad2D a, BezierQuad2D b ) => !( a == b );
		public bool Equals( BezierQuad2D other ) => P0.Equals( other.P0 ) && P1.Equals( other.P1 ) && P2.Equals( other.P2 );
		public override bool Equals( object obj ) => obj is BezierQuad2D other && Equals( other );
		public override int GetHashCode() => HashCode.Combine( p0, p1, p2 );

		public override string ToString() => $"({p0}, {p1}, {p2})";
		/// <summary>Returns a linear blend between two bézier curves</summary>
		/// <param name="a">The first spline segment</param>
		/// <param name="b">The second spline segment</param>
		/// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
		public static BezierQuad2D Lerp( BezierQuad2D a, BezierQuad2D b, float t ) =>
			new(
				Vector2.LerpUnclamped( a.p0, b.p0, t ),
				Vector2.LerpUnclamped( a.p1, b.p1, t ),
				Vector2.LerpUnclamped( a.p2, b.p2, t )
			);
		/// <summary>Splits this curve at the given t-value, into two curves that together form the exact same shape</summary>
		/// <param name="t">The t-value to split at</param>
		public (BezierQuad2D pre, BezierQuad2D post) Split( float t ) {
			Vector2 a = new Vector2(
				p0.x + ( p1.x - p0.x ) * t,
				p0.y + ( p1.y - p0.y ) * t );
			Vector2 b = new Vector2(
				p1.x + ( p2.x - p1.x ) * t,
				p1.y + ( p2.y - p1.y ) * t );
			Vector2 p = new Vector2(
				a.x + ( b.x - a.x ) * t,
				a.y + ( b.y - a.y ) * t );
			return ( new BezierQuad2D( p0, a, p ), new BezierQuad2D( p, b, p2 ) );
		}
	}
}

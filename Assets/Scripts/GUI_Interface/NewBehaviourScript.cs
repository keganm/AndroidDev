using UnityEngine;
using System.Collections;


class Vector2D<T>
{
	protected T x,y;
	
	public Vector2D(T x,T y)
	{
		this.x = x;
		this.y = y;
	}
	
	~Vector2D()
	{
		
	}
	
	public T X {
		get{ return x; }
		set{ x = value; }
	}
	
	public T Y {
		get{ return y; }
		set{ y = value; }
	}

	public T Magnitude()
	{
		return x * x + y * y;
	}

	public void Normalize()
	{
		float n = Magnitude ();
		x = x / n;
		y = y / n;
	}

	public void Rotate(float angle)
	{
		float m = Magnitude ();
		Normalize ();
		x += System.Math.Sin (angle);
		y += System.Math.Cos (angle);
		x *= m;
		y *= m;
	}

	public static override Vector2D<T> operator *(Vector2D<T> A, Vector2D<T> B)
	{
		return new Vector2D<T>(A.X * B.X, A.Y * B.Y);
	}

}

class Vector3D<T> : Vector2D<T>
{
	private float z;
	public float Z {
		get{ return z;}
		set{ z = value;}
	}
}


public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	int PickWeightedIndex( float[] weights )
	{
		float sum = 0f;
		for(int i = 0; i < weights - 1; ++i)
		{
			sum += weights[i];
		}

		System.Random rand = new System.Random ();
		float selector = (float)rand.NextDouble () * sum;

		int ret = 0;
		while (sum > 0f) {
			sum -= weights[ret];
			ret++;
		}

		return ret;
	}
}

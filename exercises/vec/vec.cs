using System;
using static System.Console;
using static System.Math;

public class vec {
    public double x, y, z; // three vector components

    // constructors
    public vec() { x = y = z = 0; }
    public vec(double x, double y, double z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    // operators
    public static vec operator*(vec u, double c) {
        return new vec(c*u.x, c*u.y, c*u.z);
    }
    public static vec operator*(double c, vec u) {
        return u*c;
    }
    public static vec operator+(vec u, vec v) {
        return new vec(u.x+v.x, u.y+v.y, u.z+v.z);
    }
    public static vec operator-(vec u) {
        return new vec(-u.x, -u.y, -u.z);
    }
    public static vec operator-(vec u, vec v) {
        return new vec(u.x-v.x, u.y-v.y, u.z-v.z);
    }

    // methods
    public void print(string s) {
        Write(s);
        WriteLine($"{x} {y} {z}");
    }
    public void print() {
        this.print("");
    }
    public double dot(vec other) {
        return this.x*other.x
            + this.y*other.y
            + this.z*other.z;
    }
    public static double dot(vec u, vec v) {
        return u.x*v.x 
            + u.y*v.y
            + u.z*v.z;
    }
    public vec cross(vec other) {
        return new vec(this.y*other.z - this.z*other.y,
                this.z*other.x - this.x*other.z,
                this.x*other.y - this.y*other.x);
    }
    public static vec cross(vec u, vec v) {
        return new vec(u.y*v.z - u.z*v.y,
                u.z*v.x - u.x*v.z,
                u.x*v.y - u.y*v.x);
    }
    public double norm() {
        return Sqrt(this.x*this.x
                + this.y*this.y
                + this.z*this.z);
    }
    public static double norm(vec u) {
        return Sqrt(u.x*u.x + u.y*u.y + u.z*u.z);
    }
    static bool approx(double a, double b, double acc=1e-9, double eps=1e-9) {
	    if(Abs(a-b) < acc) return true;
	    if(Abs(a-b) < (Abs(a)+Abs(b))*eps) return true;
	    return false;
	}
    public bool approx(vec other) {
	    if(!approx(this.x,other.x)) return false;
	    if(!approx(this.y,other.y)) return false;
	    if(!approx(this.z,other.z)) return false;
	    return true;
	}
    public static bool approx(vec u, vec v) => u.approx(v);
    public override string ToString() { return $"{x} {y} {z}"; }
} // vec


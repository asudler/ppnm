using System;
using static System.Console;

class main {
    static int Main() {
        Write("testing null vector assignment ... ");
        vec my_vector = new vec();
        my_vector.print();

        Write("testing manual vector assignment ... ");
        my_vector.x = 69;
        my_vector.y = 420;
        my_vector.z = 3.14;
        my_vector.print();

        int return_code = 0;
        bool test;
        int n = 9;
        var rnd = new Random();
        vec[] vecs = new vec[n];
        for(int i = 0; i < n; i++) {
            vecs[i] = new vec(2*rnd.NextDouble()-1,
                    2*rnd.NextDouble()-1,
                    2*rnd.NextDouble()-1);
            // vecs[i].print();
        }

        // testing methods
        Write("testing u.dot(u) = u.norm()*u.norm() ... ");
        test = true;
        for(int i = 0; i < n; i++) {
            vec u = vecs[i];
            test = test && vec.approx(u.dot(u), u.norm()*u.norm()); 
        }
        if(test) Write("passed\n");
        else { Write("FAILED\n"); return_code += 1; }

        Write("testing u.cross(v) = -v.cross(u) ... ");
        test = true;
        for(int i = 0; i < n-1; i++) {
            vec u = vecs[i]; vec v = vecs[i+1];
            test = test && u.cross(v).approx(-v.cross(u));
        }
        if(test) Write("passed\n");
        else { Write("FAILED\n"); return_code += 1; }

        Write("testing (u + v).cross(w) = u.cross(w) + v.cross(w) ... ");
        test = true;
        for(int i = 0; i < n-2; i++) {
            vec u = vecs[i]; 
            vec v = vecs[i+1];
            vec w = vecs[i+2];
            test = test && (u+v).cross(w).approx(u.cross(w) + v.cross(w));
        }
        if(test) Write("passed\n");
        else { Write("FAILED\n"); return_code += 1; }

        // conclusion
        if(return_code == 0) {
            WriteLine("all tests passed");
        }
        else {
            WriteLine("{0} tests failed. kys", return_code);
        }

        return return_code;
    } // Main
} // main

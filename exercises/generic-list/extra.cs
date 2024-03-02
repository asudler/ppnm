using System;
using static System.Console;

public class nodelist<T> {
    public node<T> first = null, current = null;
    public void add(T item) {
        if(first == null) {
            first = new node<T>(item);
            current = first;
        }
        else {
            current.next = new node<T>(item);
            current=current.next;
        }        
    }
    public void start(){ current=first; }
    public void next(){ current=current.next; }
}

public class main {
    public static int Main() {
        genlist<double> list = new genlist<double>();
        Random random = new Random();
        WriteLine("creating a new list of 5 random doubles...");
        for(int i = 0; i < 5; i++) list.add(random.NextDouble());
        for(int i = 0; i < list.size; i++) WriteLine(list[i]);
        WriteLine($"list.size is {list.size}");
        WriteLine("removing the item at position 3...");
        list.remove(3);
        for(int i = 0; i < list.size; i++) WriteLine(list[i]);
        WriteLine($"list.size is {list.size}");

        nodelist<int> a = new nodelist<int>();
        a.add(1);
        a.add(2);
        a.add(3);
        for(a.start(); a.current != null; a.next()) {
            WriteLine(a.current.item);
        }

        return 0;
    }
}

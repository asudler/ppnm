public class genlist<T> {
    public T[] data;
    public int size = 0, capacity = 8; 
    public T this[int i] { 
        get { return data[i]; } 
        set { data[i] = value; }
    } // indexer
    public genlist() { data = new T[capacity]; }
    public void add(T item) { // add item to list
        if(size == capacity) {
            T[] newdata = new T[capacity*=2];
            System.Array.Copy(data, newdata, size);
            data = newdata;
        }
        data[size] = item;
        size++;
    } // add
    public void remove(int i) { // remove item from list
        T[] newdata = new T[size-1];
        int counter = 0;
        for(int j = 0; j < size; j++) {
            if(j != i) {
                newdata[counter] = data[j];
                counter += 1;
            }
        }
        data = newdata;
        size = size - 1;
    }   
} // genlist

public class node<T>{
	public T item;
	public node<T> next;
	public node(T item){this.item=item;}
}

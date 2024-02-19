public class genlist<T> {
    public T[] data;
    public int size => data.Length; // property
    public T this[int i] { 
        get { return data[i]; } 
        set { data[i] = value; }
    } // indexer
    public genlist() { data = new T[0]; }
    public void add(T item) {
        T[] newdata = new T[size+1];
        for(int i = 0; i < size; i++) newdata[i] = data[i];
        newdata[size] = item;
        data = newdata;
    }   
} // genlist


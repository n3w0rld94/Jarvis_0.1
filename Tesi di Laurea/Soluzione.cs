public class FFANN{
	private layer[] Layers;
	//...All other stuff
	
	public void AddLayer(int n){
		Layers[n] = new Layer(this,...);
	}
}

public class Layer{
	public FFANN Parent;
	private Perceptron[] perceptrons;
	
	public Layer(FFANN parent){
		Parent = parent;
	}
	public void AddPerce(int n){
		perceptrons[n] = new Perceptron(this,...);
	}
}

public class Perceptron{
	public Layer Parent;
	
	public Perceptron(Layer parent){
		Parent = parent;
	}
}

public class FFANN{
	private layer[] Layers;
	//...All other stuff
	
	public void AddLayer(int n){
		Layers[n] = new Layer(this,...);
	}
}

public class Layer{
	public FFANN Parent;
	
	public Layer(FFANN parent){
		Parent = parent;
	}
}

public class Perceptron{
	public Layer Parent;
	
	public Perceptron(Layer parent){
		Parent = parent;
	}
}

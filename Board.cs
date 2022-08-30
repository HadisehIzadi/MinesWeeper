using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
	
	// variables : 
	
	public Tilemap tilemap
	{ get ; private set;}
	
	public Tile tileUnknown;
	public Tile tileEmpty;
	public Tile tileMine;
	public Tile tileExploded;
	public Tile tileFalg;
	public Tile tileNum1;
	public Tile tileNum2;
	public Tile tileNum3;
	public Tile tileNum4;
	public Tile tileNum5;
	public Tile tileNum6;
	public Tile tileNum7;
	public Tile tileNum8;
	
	
	// awake
	private void Awake()
	{
		tilemap = GetComponent<Tilemap>();
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    
    
    public void Draw(Cell[,] state)
    {
    	int width = state.GetLength(0);
    	int height = state.GetLength(1);
    	
    	
    	
  
    	// x
    	for(int x = 0 ;  x < width; x++)

    	{
    		// y
    		for(int y = 0 ; y < height ; y++)
    		{
    			Cell cell = state[x,y];
    			tilemap.SetTile(cell.position , GetTile(cell)); 
    		}
    	}
    }
    
    
    
    
    
    private Tile GetTile(Cell cell)
    {
    	if(cell.revealed) // true
    	{
    		return GetRevealedTile(cell);
    	}
    	else if(cell.flagged) // if it is a flag
    	{
    		return tileFalg;
    		
    	}
    	else{
    		return tileUnknown;
    	}
    	
    	
    	return null;
    }
    
    
    
    private Tile GetRevealedTile(Cell cell)
    {
    	switch(cell.type)
    	{
    			case Cell.Type.Empty: return tileEmpty;
    			case Cell.Type.Mine:
    			if(cell.exploded == true)
    				return tileExploded;
    			else
    				return tileMine;
    			break;
    			//return cell.exploded ? tileExploded : tileMine;
    			case Cell.Type.Number: return GetNumberTile(cell);
    			default : return null;
    			
    	}
    	
    }
    
    
    private Tile GetNumberTile(Cell cell)
    {
    	switch (cell.number)
    	{
    			case 1 : return tileNum1;
    			case 2 : return tileNum2;
    			case 3 : return tileNum3;
    			case 4 : return tileNum4;
    			case 5 : return tileNum5;
    			case 6 : return tileNum6;
    			case 7 : return tileNum7;
    			case 8 : return tileNum8;
    			
    			
    			default : return null;
    	}
    }
    
}

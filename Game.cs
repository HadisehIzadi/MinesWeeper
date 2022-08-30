using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : MonoBehaviour
{
	
	//variables
	public int width = 16;
	public int height = 16;
	
	private Board board;
	
	private Cell[,] state;
	
	public int mineCount = 32;
	
	private bool gameover;
	
	public Text ClickManager;
	private int ClickCount ;

	//******************************
	
	//awake
	private void Awake()
	{
		board = GetComponentInChildren<Board>();
	}
	
	
    // Start is called before the first frame update
    void Start()
    {
    	
    	NewGame();
    	
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetKeyDown(KeyCode.R))
    		NewGame();
    	
    	if(!gameover){
    	if(Input.GetMouseButtonDown(1))
    	{
    		CountUserClicks();
    		Flag();
    	}
    	else if(Input.GetMouseButtonDown(0))
    	{
    		CountUserClicks();
    		Reveal();
    	}
    	
    	
    	}
    }
    
    
    
    private void NewGame()
    {
    	ClickCount = 0 ;
    	gameover = false;
    	state = new Cell[width , height];
    	generateCells();
    	GenerateMines();
    	GenerateNumbers();
    	Camera.main.transform.position = new Vector3(width/2f , height/2f , -1f);
    	board.Draw(state);
    	
    }
    
    private void generateCells()
    {
    	// x
    	for(int x = 0 ;  x < width; x++)

    	{
    		// y
    		for(int y = 0 ; y < height ; y++)
    		{
    			Cell cell = new Cell();
    			
    			cell.position = new Vector3Int(x , y , 0);
    			cell.type = Cell.Type.Empty;
    			
    			state[x,y] = cell;
    			
    		}
    	}
    }
    
    
    private void GenerateMines()
    {
    	for(int i = 0 ; i < mineCount; i++)
    	{
    		int x = Random.Range(0,width);
    		int y = Random.Range(0,height);
    		
    		
    		while(state[x,y].type == Cell.Type.Mine)
    		{
    			x++;
    			if(x >= width)
    			{
    				x = 0 ;
    				y++;
    				
    				if(y >= height)
    				{
    					y = 0 ;
    				}
    			}
    			
    		}
    		
    		
    		state[ x , y ].type = Cell.Type.Mine;
    		//state[ x , y ].revealed = true;
    	}
    }
    
    
    
    private void GenerateNumbers()
    {
    	for(int x = 0 ; x < width ; x++)
    	{
    		for(int y = 0 ; y <height ; y++)
    		{
    			Cell cell = state[x,y];
    			
    			if(cell.type == Cell.Type.Mine)
    			{
    				continue;
    			}
    			cell.number = CountMines(x,y);
    			
    			if(cell.number >0)
    				cell.type = Cell.Type.Number;
    			
    			
    			//cell.revealed = true;
    			state[x,y] = cell;
    		}
    	}
    }
    
    
    private int CountMines(int cellx , int celly)
    {
    	int count = 0 ;
    	for(int adjacentX = -1 ; adjacentX <= 1 ; adjacentX++)
    	{
    		for(int adjacentY = -1 ; adjacentY <= 1 ; adjacentY++)
    		{
    			if(adjacentX ==0 && adjacentY==0)
    				continue;
    			
    			int x = cellx+adjacentX;
    			int y = celly+adjacentY;
    			
    			if(x<0 || x >= width || y<0 ||y>=height )
    				continue;
    			
    			// * state[x,y]
    			if(GetCell(x,y).type == Cell.Type.Mine)
    			{
    				count++;
    			}
    		}
    	}
    	
    	
    	return count;
    }
    
    
    private void Flag()
    {
    	Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	Vector3Int cellpos = board.tilemap.WorldToCell(worldPos);
    	
    	
    	Cell cell = GetCell(cellpos.x , cellpos.y);
    	
    	
    	if(cell.type == Cell.Type.Invalid || cell.revealed)
    	{
    		return;
    		
    	}
    	
    	cell.flagged = !cell.flagged;
    	
    	state[cellpos.x , cellpos.y] = cell;
    	
    	board.Draw(state);
    }
    
    
    private Cell GetCell(int x , int y)
    {
    	if(IsValid(x,y))
    		return state[x,y];
    	else
    		return new Cell();
    }
    
    
    private bool IsValid(int x , int y)
    {
    	return x>=0 && x <width && y>=0 && y <height;
    }
    
    
    private void Reveal()
    {
    	Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	Vector3Int cellpos = board.tilemap.WorldToCell(worldPos);
    	
    	
    	Cell cell = GetCell(cellpos.x , cellpos.y);
    	
    	 if(cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged)
    	{
    	 	CheckWin();
    		return;
    		
    	}
    	 
    	 if(cell.type ==Cell.Type.Empty)
    	 {
    	 	
    	 	Flood(cell);
    	 	CheckWin();
    	 }
    	 
    	 if(cell.type ==Cell.Type.Mine)
    	 {
    	 	cell.exploded = true;
    	 	Exploded(cell);
    	 }
    	 
    	 cell.revealed = true;
    	 state[cellpos.x , cellpos.y] = cell;
    	 board.Draw(state);
    	
    }
    
    
    private void Flood(Cell cell)
    {
    	if(cell.revealed) 
    		return;
    	if(cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid)
    		return;
    	
    	
    	cell.revealed = true;
    	state[cell.position.x , cell.position.y] = cell;
    	
    	if(cell.type == Cell.Type.Empty)
    	{
    		Flood(GetCell(cell.position.x -1 , cell.position.y));
    		Flood(GetCell(cell.position.x +1 , cell.position.y));
    		Flood(GetCell(cell.position.x  , cell.position.y -1));
    		Flood(GetCell(cell.position.x  , cell.position.y +1));
    	}
    }
    
    
    private void Exploded(Cell cell)
    {
    	Debug.Log("Game Over");
    	gameover = true;
    	
    	cell.revealed = true;
    	
    	
    	state[cell.position.x , cell.position.y] = cell;
    	
    	
    	// to show other mines in game 
    	for(int x = 0 ; x < width ; x++)
    	{
    		for(int y = 0 ; y <height ; y++)
    		{
    			cell = state[x,y];
    			if(cell.type == Cell.Type.Mine)
    			{
    				cell.exploded = true;
    				cell.revealed = true;
    				state[x,y] = cell;
    			}
    		}
    	}
    	
    }
    
    
    
    private void CheckWin()
    {
    	for(int x = 0 ; x < width ; x++)
    	{
    		for(int y = 0 ; y <height ; y++)
    		{
    			Cell cell = state[x,y];
    			if(cell.type != Cell.Type.Mine && !cell.revealed)	
    			{
    				return;
    			}
    			
    		}	
    	}
    	
    	Debug.Log("Win");
    	gameover = true; // finish game
    	
    	
    	 // to show other mines in game 
    	for(int x = 0 ; x < width ; x++)
    	{
    		for(int y = 0 ; y <height ; y++)
    		{
    			Cell cell = state[x,y];
    			if(cell.type == Cell.Type.Mine)
    			{
    				cell.flagged = true;
    				state[x,y] = cell;
    			}
    		}
    	}
    	
    	
    }
    
    
    public void Restart()
    {
    	NewGame();
    }
    
    public void Exit()
    {
    	gameover = true;
    	Application.Quit();

    }
    
    
    public void CountUserClicks()
    {
    	ClickCount++;
    	ClickManager.text = ClickCount.ToString();
    }
    

}

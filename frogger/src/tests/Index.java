package tests;
import static org.junit.Assert.*;
import java.util.Vector;

import javax.swing.JOptionPane;

import org.junit.*;
import frogger.*;

//Dar klausimas, o kodel ems darom su JUnit 4 ??? kodel mes su naujesne versija nedarom??
//ten bent kodo tiek refacotrint nereiktu


//TODO - ka galima istestint listas:
/*
 * FroggerComponent.java - {
 * 	1. level
 * 	2. life
 * 	3. score
 * 	4.FroggerState - all states
 * 	5. lab1, lab2, lab3 - skirtinguose leveliuose?
 * }
 * FroggerLevelEngine - su update kaip su trafic intersects, moveup, down ir .t.t. tai cia realiai paliestu ir Frog.java
 *  TrafficPattern - su speed ar nesugriaus zaidimo, nes limito setinimui speedo nera realiai kaip ir bounds, o intersects su frog jau butu ankstesniame testas
 * 
 * */
public class Index {
//	@Test
//	public void antras() {
//		FroggerLevelEngine engine;
//		Vector<FroggerLevel> levels = new Vector();
//		int level = 0;
////		TrafficPattern[] traffic;
////		Frog frog;
//		
//		engine = new FroggerLevelEngine(levels.get(level));
//		
//		
//		
//	}
	
	
	@Test
	public void ScoreChange() {
		PlayFrogger froggerPlay =  new PlayFrogger(); 
		
		FroggerState state = FroggerState.MOVING;
		state = FroggerState.WON;
		FroggerComponent comp = new FroggerComponent();
		int score = comp.score;
		
		comp.engine.state = FroggerState.WON;
		
		try {
			comp.update();
			assertEquals(comp.score, 300);
			
		} catch (InterruptedException e) {
			fail("Test failed: InterruptedException");
			e.printStackTrace();
			
		}
	}
	

	@Test
	public void LifeChangeOnHit() {
		PlayFrogger froggerPlay =  new PlayFrogger(); 
		FroggerComponent comp = new FroggerComponent();
		int lives = comp.life - 1; //final result mus be - 1
		comp.engine.state = FroggerState.HIT;
		try {
			comp.update();
			assertEquals(lives, comp.life);
			
		} catch (InterruptedException e) {
			fail("Test failed: InterruptedException");
			e.printStackTrace();
		}
		
	}
	
	@Test
	public void LevelChange() {
		PlayFrogger froggerPlay =  new PlayFrogger(); 
		FroggerComponent comp = new FroggerComponent();
		int level = comp.level + 1; //next level mus be + 1
		comp.engine.state = FroggerState.WON;
		
		try {
			comp.update();
			assertEquals(level, comp.level);
			
		} catch (InterruptedException e) {
			fail("Test failed: InterruptedException");
			e.printStackTrace();
		}
	}
	
	
	//problema, kad kazkodel dar per tuos update ir pats game dabar leidzias per testa :(((
	@Test
	public void NoMoreLives() {
		//after death need to check if correct values are assigned
		//main assert mus be level changed to lvl 1 from what it used to be
		PlayFrogger froggerPlay =  new PlayFrogger(); 
		FroggerComponent comp = new FroggerComponent();
		int level = 0; //final result 0
		comp.engine.state = FroggerState.WON;
		
		//need to reach further level from 1
		try {
			while(comp.level != 8) {
				comp.update();
				comp.engine.state = FroggerState.WON;
			}
			
		} catch (InterruptedException e) {
			fail("Test failed: InterruptedException");
			e.printStackTrace();
		}
		
		//after further level test if it resets
		comp.engine.state = FroggerState.HIT;
		comp.life = 0; 
		try {
			comp.update(); //after update pop up comes up
			//pop up refactored for error and victory
			assertEquals(level, comp.level);
			
		} catch (InterruptedException e) {
			fail("Test failed: InterruptedException");
			e.printStackTrace();
		}
		
	}
	
	@Test
	public void onVictory(){
//		fail("Not yet implemented");
		PlayFrogger froggerPlay =  new PlayFrogger(); 
		FroggerComponent comp = new FroggerComponent();
		comp.engine.state = FroggerState.WON;
		comp.level = 89;
		
		try {
			System.out.println(comp.level);
			System.out.println(comp.levels.size());
			comp.update(); //after update pop up comes up
			System.out.println(comp.level);
			System.out.println(comp.levels.size()); //it fails because it looks like levels are only 9,
			//but level size output is 90
			//while levels are 9
			//so it goes out of range if i set level to 91 and does not react if set to level 9
			//aslo can not catch system.exit(0)- the program just exits
			assertEquals(1, comp.endGameAfterVicotory);
			
		} catch (InterruptedException e) {
			e.printStackTrace();
			fail("Test failed: InterruptedException");
		}
	}
	
	
	@Test
	public void CollisionCheck() {
		fail("Not yet implemented");
	}
	
	
}

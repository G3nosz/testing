package tests;
import static org.junit.Assert.*;

import java.util.Vector;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import frogger.*;


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

	@Test
	public void test_JUnit() {
		System.out.println("Tai cia tekstas");
		String str1 = "Tai cia testas";
		String str2 = "Tai cia testas";
		assertEquals(str2, str1);
	}
	
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
	public void trecias() {
		PlayFrogger froggerPlay =  new PlayFrogger(); 
		
		FroggerState state = FroggerState.MOVING;
		state = FroggerState.WON;
		FroggerComponent comp = new FroggerComponent();
		int score = comp.score;
		
		comp.engine.state = FroggerState.WON;
		System.out.println(comp.score);
		
		try {
			comp.update();
			System.out.println(comp.score);
			assertEquals(comp.score, 300);
			
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			assertEquals(comp.score, 0);
			e.printStackTrace();
			
		}
	}
	
	
	
}

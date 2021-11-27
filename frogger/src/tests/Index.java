package tests;
import static org.junit.Assert.*;

import java.util.Vector;

import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import frogger.*;

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
package tests;

import static org.junit.Assert.fail;
import static org.junit.jupiter.api.Assertions.assertEquals;

import org.junit.jupiter.api.Test;

import frogger.FroggerComponent;
import frogger.FroggerState;
import frogger.PlayFrogger;

//TODO - ka galima istestint listas:
/*
 * FroggerComponent.java - {
 * 	4.FroggerState - all states   -- liko kazka su moving pasidaryt
 * }
 * FroggerLevelEngine - su update kaip su trafic intersects, moveup, down ir .t.t. tai cia realiai paliestu ir Frog.java
 *  TrafficPattern - su speed ar nesugriaus zaidimo, nes limito setinimui speedo nera realiai kaip ir bounds, o intersects su frog jau butu ankstesniame testas
 * 
 * */
public class Index {

	
	
	//tai jau cia kazkas su paciu kodu idomaus, nes ten yra taip, kad leveliu realiai yra tik 9
	//bet kaip varle laimi jau perejus visus lygius, flag vistiek nesusiaktyvina
	@Test
	public void onVictory(){
		PlayFrogger froggerPlay =  new PlayFrogger(); 
		FroggerComponent comp = new FroggerComponent();
		comp.engine.state = FroggerState.WON;

		try {
			while(comp.level <= comp.levels.size()) {
				comp.update();
				comp.engine.state = FroggerState.WON;
			}
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

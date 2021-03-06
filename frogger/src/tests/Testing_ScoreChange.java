package tests;

import static org.junit.Assert.fail;
import static org.junit.jupiter.api.Assertions.assertEquals;

import java.util.stream.Stream;

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;
import frogger.*;

public class Testing_ScoreChange {
	@ParameterizedTest
	@MethodSource("inputStream")
	void LevelChanges(int victory, int expected) {
		PlayFrogger froggerPlay = new PlayFrogger();
		FroggerComponent comp = new FroggerComponent();
		comp.engine.state = FroggerState.WON;
		
		try {
			while(victory > 0) {
				comp.update();
				comp.engine.state = FroggerState.WON;
				victory--;
			}
			
			assertEquals(expected, comp.score);
		} catch (InterruptedException e) {
			fail("Test failed: InterruptedException");
			e.printStackTrace();
		}
	}
	
	private static Stream<Arguments> inputStream(){
		return Stream.of(
			Arguments.of(0, 0),
			Arguments.of(1, 300),
			Arguments.of(2, 600),
			Arguments.of(3, 900),
			Arguments.of(4, 1200),
			Arguments.of(5, 1500),
			Arguments.of(6, 1800),
			Arguments.of(7, 2100),
			Arguments.of(8, 2400),
			Arguments.of(9, 2700)
		);
	}
}

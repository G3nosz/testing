package tests;

import static org.junit.Assert.fail;
import static org.junit.jupiter.api.Assertions.assertEquals;

import java.util.stream.Stream;

import javax.swing.JLabel;

import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

import frogger.FroggerComponent;
import frogger.FroggerState;
import frogger.PlayFrogger;

public class Testing_Game_LabelChange {
	@ParameterizedTest
	@MethodSource("inputStream")
	void LevelChanges(int hit, JLabel expected) {
		PlayFrogger froggerPlay = new PlayFrogger();
		FroggerComponent comp = new FroggerComponent();
		comp.engine.state = FroggerState.HIT;
		
		JLabel expectedLabel = expected;
		
		try {
			while(hit > 0) {
				comp.update();
				hit--;
				comp.engine.state = FroggerState.HIT;
			}
			
			assertEquals(expectedLabel.getText(), froggerPlay.lab2.getText());
		} catch (InterruptedException e) {
			fail("Test failed: InterruptedException");
			e.printStackTrace();
		}
	}
	
	private static Stream<Arguments> inputStream(){
		return Stream.of(
			Arguments.of(0,  new JLabel("♥♥♥♥♥")),
			Arguments.of(1, new JLabel("♥♥♥♥")),
			Arguments.of(2, new JLabel("♥♥♥")),
			Arguments.of(3, new JLabel("♥♥")),
			Arguments.of(4, new JLabel("♥"))
		);
	}
}

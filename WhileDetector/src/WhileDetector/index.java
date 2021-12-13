package WhileDetector;

import javax.xml.crypto.dsig.DigestMethod;

import org.apache.bcel.classfile.JavaClass;
import org.apache.bcel.classfile.Method;
import edu.umd.cs.findbugs.BugInstance;
import edu.umd.cs.findbugs.BugReporter;
import edu.umd.cs.findbugs.ba.JavaClassAndMethod;
import edu.umd.cs.findbugs.bcel.OpcodeStackDetector;



public class index extends OpcodeStackDetector  {
	private BugReporter bugReporter;
	public index(BugReporter bugReporter) {
		this.bugReporter = bugReporter;
	};
	
	public void visit(JavaClassAndMethod someObj) {

		// iterate through all class methods
		for (Method setterMethod : someObj.getMethods()) {
			// do we have a getter?
			if (method.getName().startsWith("get")) {
				String setterName = "set" + method.getName().substring(3);
				boolean found = false;

				// iterate through all class methods while searching for the
				// setter
				for (Method setterMethod : someObj.getMethod()) {
					// do we have a getter?
					if (setterMethod.getName().equals(setterName)) {
						found = true;
						break;
					}
				}

				// so no setter was found
				if (false == found) {
					// report the issue
					bugReporter.reportBug(new BugInstance(this, "GETSET_BUG", NORMAL_PRIORITY).addClassAndMethod(this)
				.addString("").addSourceLine(this));
	}
}
}

super.visit(someObj);
}

public void sawOpcode(int arg0) {
}


}

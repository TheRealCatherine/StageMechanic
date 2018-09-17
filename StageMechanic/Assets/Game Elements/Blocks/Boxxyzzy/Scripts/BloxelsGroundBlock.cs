using MoonSharp.Interpreter;

public class BloxelsGroundBlock : AbstractBloxelsBlock
{

	public override string TypeName
	{
		get
		{
			return "Ground";
		}

		set
		{
			throw new System.NotImplementedException();
		}
	}


	public override void Awake()
	{
		base.Awake();
		LogController.Log(MoonSharpFactorial().ToString());
	}

	double MoonSharpFactorial()
	{
		string script = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end

		return fact(5)";

		DynValue res = Script.RunString(script);
		return res.Number;
	}

	public override void ApplyTheme(AbstractBlockTheme theme)
	{
		throw new System.NotImplementedException();
	}
}

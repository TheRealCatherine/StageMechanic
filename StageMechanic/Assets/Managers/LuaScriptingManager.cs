using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Platforms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaScriptingManager : MonoBehaviour
{
	public static LuaScriptingManager Instance;
	public const string Keywords = @"\b(and|break|do|else|elseif|end|false|for|function|if|in|local|nil|not|or|repeat|return|then|true|until|while)\b";
	public const string Classes = @"\b(blockmanager|itemmanager|playermanager|audiomanager|visualeffectsmanager|skyboxmanager|alert|coroutine|string|utf8|table|math|io|file|os|debug|vector3)\b";
	public const string Variables = @"\b(block|item|player|PI)\b";
	public const string Properties = @"\b(\.name|\.type|\.pos|\.weight|\.gravity|\.motionstate|\.grounded|\.group"
		+ @"|blockmanager\.count|blockmanager\.types|itemmanager\.count|itemmanager\.types|playermanager\.count"
		+ @"|\.owningblock|\.owningplayer|\.icon|\.collectable|\.uses|\.trigger|\.score"
		+ @"|\.facing|\.allstates|\.state|\.item"
		+ @"|\.x|\.y|\.z"
		+ @")\b";
	public const string Functions = @"\b(assert|collectgarbage|error|_G|getmetatable|ipairs|next|pairs|pcall|print|rawequal|rawget|rawlen|rawset|select|setmetatable|ronumber|tostring|type|_VERSION|xpcall"
		+ @"|coroutine\.create|coroutine\.isyieldable|coroutine\.resume|coroutine\.running|coroutine\.status|coroutine\.wrap|coroutine\.yield"
		+ @"|package\.congig|package\.cpath|package\.loaded|package\.loadlib|package\.path|package\.preload|package\.searchers|package\.searchpath" 
		+ @"|string\.byte|string\.char|string\.dump|string\.find|string\.format|string\.gmatch|string\.gsub|string\.len|string\.lower|string\.match|string\.pack|string\.packsize|string\.rep|string\.reverse|string\.sub|string\.unpack|string\.upper"
		+ @"|utf8\.char|utf8\.charpattern|utf8\.codes|utf8\.codepoint|utf8\.len|utf8\.offset"
		+ @"|table\.concat|table\.insert|table\.move|table\.pack|table\.remove|table\.sort|table\.unpack"
		+ @"|math\.abs|math\.acos|math\.asin|math\.atan|math\.ceil|math\.cos|math\.deg|math\.exp|math\.floor|math\.fmod|math\.hugh|math\.log|math\.max|math\.maxinteger|math\.min|math\.mininteger|math\.modf|math\.pi|math\.rad|math\.random|math\.randomseed|math\.sin|math\.sqrt|math\.tan|math\.tointeger|math\.type|math\.ult"
		+ @"|os\.clock|os\.date|os\.difftime|os\.time"
		+ @"|\.play"
		+ @"|\.canbepulled|\.pullweight|\.pull|\.canbepushed|\.pushweight|\.push|\.set|\.unset|\.get|\.run"
		+ @"|blockmanager\.at|blockmanager\.clear|blockmanager\.create|blockmanager\.destroy|blockmanager\.getall"
		+ @"|itemmanager\.at|itemmanager\.clear|itemmanager\.create|itemmanager\.destroy|itemmanager\.getall"
		+ @"|\.applygravity|\.turnaround|\.turn|\.turnright|\.turnleft|\.takedamage|\.useitem"
		+ @"|playermanager\.player|playermanager\.clear|playermanager\.showall|playermanager\.hideall|playermanager\.at"
		+ @"|skyboxmanager\.count|skyboxmanager\.index|skyboxmanager\.next"
		+ @"|vector3.add|vector3.sub"
		+ @"|alert\.info"
		+ @")\b";

	public const string Blacklist = @"\b("
		+ @"load|loadsafe|loadfile|loadfilesafe|dofile|require"
		+ @"|os\.execute|os\.exit|os\.getenv|os\.remove|os\.rename|os\.setlocale|os\.tmpname"
		+ @"|io\.close|io\.flush|io\.input|io\.lines|io\.open|io\.output|io\.popen|io\.read|io\.tmpfile|io\.type|io\.write|file:close|file:flush|file:lines|file:read|file:seek|file:setvbuf|file:write"
		+ @"|debug\.debug|debug\.gethook|debug\.getinfo|debug\.getlocal|debug\.getmetatable|debug\.getregistry|debug\.getupvalue|debug\.getuservalue|debug\.sethook|debug\.setlocal|debug\.setmetatable|debug\.setupvalue|debug\.setuservalue|debug\.traceback|debug\.upvalueid|debug\.upvaluejoin"
		+ @"|_ALERT|_ERRORMESSAGE|_INPUT|_OUTPUT|_STDERR|_STDIN|STDOUT|PROPMPT"
		+ @"\b)";

	public const string Types = @"\b(nil|boolean|number|string|userdata|function|thread|table)\b";

	private void Awake()
	{
		Instance = this;
	}

	void Start()
    {
		Script.GlobalOptions.Platform = new LimitedPlatformAccessor();

		UserData.RegisterProxyType<LuaProxy_AbstractBlock, AbstractBlock>(r => new LuaProxy_AbstractBlock(r));
		UserData.RegisterProxyType<LuaProxy_AbstractItem, AbstractItem>(r => new LuaProxy_AbstractItem(r));
		UserData.RegisterProxyType<LuaProxy_AbstractPlayerCharacter, AbstractPlayerCharacter>(r => new LuaProxy_AbstractPlayerCharacter(r));
		UserData.RegisterProxyType<LuaProxy_BlockManager, BlockManager>(_ => new LuaProxy_BlockManager());
		UserData.RegisterProxyType<LuaProxy_ItemManager, ItemManager>(_ => new LuaProxy_ItemManager());
		UserData.RegisterProxyType<LuaProxy_PlayerManager, PlayerManager>(_ => new LuaProxy_PlayerManager());
		UserData.RegisterProxyType<LuaProxy_AudioEffectsManager, AudioEffectsManager>(_ => new LuaProxy_AudioEffectsManager());
		UserData.RegisterProxyType<LuaProxy_SkyboxManager, SkyboxManager>(_ => new LuaProxy_SkyboxManager());
		UserData.RegisterType<Sprite>();
		UserData.RegisterType<Alert>();
		UserData.RegisterType<VectorMath>();
		LuaCustomConverters.RegisterAll();
	}

	public static Script BaseScript
	{
		get
		{
			Script script = new Script(CoreModules.Preset_SoftSandbox);
			DynValue blockManager = UserData.Create(BlockManager.Instance);
			DynValue itemManager = UserData.Create(ItemManager.Instance);
			DynValue playerManager = UserData.Create(PlayerManager.Instance);
			DynValue audioEffectsManager = UserData.Create(AudioEffectsManager.Instance);
			DynValue skyboxManager = UserData.Create(SkyboxManager.Instance);
			DynValue alert = UserData.Create(new Alert());
			DynValue vectormath = UserData.Create(new VectorMath());
			script.Globals.Set("blockmanager", blockManager);
			script.Globals.Set("itemmanager", itemManager);
			script.Globals.Set("playermanager", playerManager);
			script.Globals.Set("soundeffectsmanager", audioEffectsManager);
			script.Globals.Set("skyboxmanager", skyboxManager);
			script.Globals.Set("alert", alert);
			script.Globals.Set("vector3", vectormath);
			return script;
		}
	}

	//TODO show error messages
	public static DynValue RunScript(Script environment, string code)
	{
		if (string.IsNullOrWhiteSpace(code))
		{
			Debug.Log("Can't run empty script!");
			return null;
		}
		try
		{
			return environment.DoString(code);
		}
		catch (SyntaxErrorException ex)
		{
			string message = "Syntax Error!\n" + ex.DecoratedMessage;
			UIManager.ShowMessage(message, 5);
			Debug.Log(message);
		}
		catch (InternalErrorException ex)
		{
			string message = "Lua Interpreter Error!\n" + ex.DecoratedMessage;
			UIManager.ShowMessage(message, 5);
			Debug.Log(message);
		}
		catch (DynamicExpressionException ex)
		{
			string message = "Dynamic Expression Error!\n" + ex.DecoratedMessage;
			UIManager.ShowMessage(message, 5);
			Debug.Log(message);
		}
		catch (ScriptRuntimeException ex)
		{
			string message = "Runtime Error!\n" + ex.DecoratedMessage;
			UIManager.ShowMessage(message, 5);
			Debug.Log(message);
		}
		catch (System.Exception ex)
		{
			string message = "Stage Mechanic Engine Error!\n";
			UIManager.ShowMessage(message, 5);
			Debug.Log(ex.Message);
		}
		return null;
	}
}

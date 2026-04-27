using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Saves;
using System.Reflection;
using MegaCrit.Sts2.Core.Commands;
using TH_Rin.Scrpits.Cards;

namespace TH_Rin.Scripts.Main
{
	[ModInitializer("Init")]
	public class RinInit
	{
   private const string ModSfxPrefix = "mod_sfx://";

		public static string ToModSfxPath(string localPath)
		{
			return ModSfxPrefix + localPath;
		}
	private static Harmony? _harmony;
	public static void Init()
	{
		 TryRegisterGodotScriptAssembly();
		_harmony = new Harmony("TH_Rin");
		_harmony.PatchAll();
		InitTools();
		Log.Debug("Rin mod has been loaded successfully");
	}
    public static bool InitTools()
	{
        Tools._monsterToCorpseCard.Clear();
		Tools._monsterToCorpseCard.Add((typeof(LeafSlimeM), typeof(SlimeCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(LeafSlimeS), typeof(SlimeCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TwigSlimeM), typeof(SlimeCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TwigSlimeS), typeof(SlimeCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SlimedBerserker), typeof(SlimeCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(ShrinkerBeetle), typeof(ShrinkerBeetleCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(VineShambler), typeof(VineShamblerCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(CubexConstruct), typeof(CubexConstructCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Nibbit), typeof(NibbitCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Inklet), typeof(InkletCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SnappingJaxfruit), typeof(SnappingJaxfruitCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(FuzzyWurmCrawler), typeof(FuzzyWurmCrawlerCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SlitheringStrangler), typeof(SlitheringStranglerCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Mawler), typeof(MawlerCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Wriggler), typeof(WriggleCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Vantom), typeof(VantomCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Fogmog), typeof(FogmogCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Flyconid), typeof(FlyconidCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Byrdonis), typeof(ByrdonisCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(BygoneEffigy), typeof(BygoneEffigyCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(PhrogParasite), typeof(PhrogParasiteCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(CeremonialBeast), typeof(CeremonialBeastCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(KinFollower), typeof(KinFollowerCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(KinPriest), typeof(KinPriestCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(AxeRubyRaider), typeof(HumanCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(AssassinRubyRaider), typeof(HumanCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(BruteRubyRaider), typeof(HumanCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(CrossbowRubyRaider), typeof(HumanCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TrackerRubyRaider), typeof(HumanCorpse)));
        Tools._monsterToCorpseCard.Add((typeof(LivingFog), typeof(SlivingFogCorpse)));
	    Tools._monsterToCorpseCard.Add((typeof(Toadpole), typeof(ToadpoleCorpse)));
        Tools._monsterToCorpseCard.Add((typeof(CalcifiedCultist), typeof(CultistCorpse)));
        Tools._monsterToCorpseCard.Add((typeof(DampCultist), typeof(CultistCorpse)));
        Tools._monsterToCorpseCard.Add((typeof(DevotedSculptor), typeof(CultistCorpse)));
	    Tools._monsterToCorpseCard.Add((typeof(FossilStalker), typeof(FossilStalkerCorpse)));
	    Tools._monsterToCorpseCard.Add((typeof(Seapunk), typeof(SeapunkCorpse)));
	    Tools._monsterToCorpseCard.Add((typeof(SlivingFogCorpse), typeof(SlivingFogCorpse)));
	    Tools._monsterToCorpseCard.Add((typeof(FatGremlin), typeof(GermlinCorpse)));
	    Tools._monsterToCorpseCard.Add((typeof(SneakyGremlin), typeof(GermlinCorpse)));
        Tools._monsterToCorpseCard.Add((typeof(TerrorEel), typeof(TerrorEelCorpse)));
        Tools._monsterToCorpseCard.Add((typeof(SoulFysh), typeof(SoulFyshCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SludgeSpinner), typeof(SludgeSpinnerCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TwoTailedRat), typeof(TwoTailedRatCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(WaterfallGiant), typeof(WaterfallGaintCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(LagavulinMatriarch), typeof(LagavulinMatriarchCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SewerClam), typeof(SewerClamCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(PhantasmalGardener), typeof(PhantasmalGardenerCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SkulkingColony), typeof(SkulkingColonyCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(HauntedShip), typeof(HauntedShipCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(PunchConstruct), typeof(PunchConstructCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(CorpseSlug), typeof(CorpseSlugCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(KnowledgeDemon), typeof(KnowledgeDemonCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TheInsatiable), typeof(TheInsatiableCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(LouseProgenitor), typeof(LouseProgenitorCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(HunterKiller), typeof(HunterKillerCorpse)));	
		Tools._monsterToCorpseCard.Add((typeof(SlumberingBeetle), typeof(SlumberingBeetleCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(ThievingHopper), typeof(ThievingHopperCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Exoskeleton), typeof(ExoskeletonCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Myte), typeof(MyteCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Ovicopter), typeof(OvicopterCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(ToughEgg), typeof(SmallOvicopterCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Entomancer), typeof(EntomancerCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SpinyToad), typeof(SpinyToadCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TheObscura), typeof(TheObscureCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(BowlbugEgg), typeof(BowlbugCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(BowlbugNectar), typeof(BowlbugCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(BowlbugRock), typeof(BowlbugCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(BowlbugSilk), typeof(BowlbugCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Chomper), typeof(ChomperCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(InfestedPrism), typeof(InfestedPrismCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(DecimillipedeSegmentFront), typeof(DecimillipedeSegmentCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(DecimillipedeSegmentMiddle), typeof(DecimillipedeSegmentCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(DecimillipedeSegmentBack), typeof(DecimillipedeSegmentCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Rocket), typeof(RocketCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Crusher), typeof(CrusherCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Tunneler), typeof(TunnelerCorpse)));
        Tools._monsterToCorpseCard.Add((typeof(TestSubject), typeof(TestSubjectCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TorchHeadAmalgam), typeof(TorchHeadAmalgamCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Queen), typeof(QueenCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SoulNexus), typeof(SoulNexusCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(FlailKnight), typeof(FlailKnightCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(MagiKnight), typeof(MagiKnightCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(SpectralKnight), typeof(SpectralKnightCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(MechaKnight), typeof(MechaKnightCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(ScrollOfBiting), typeof(ScrollOfBitingCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Zapbot), typeof(ZapbotCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Noisebot), typeof(NoisebotCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Stabbot), typeof(StabbotCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Guardbot), typeof(GuardbotCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Fabricator), typeof(FabricatorCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TheLost), typeof(TheLostCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TheForgotten), typeof(TheForgottenCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(FrogKnight), typeof(FrogKnightCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(OwlMagistrate), typeof(OwlMagistrateCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(LivingShield), typeof(LivingShieldCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TurretOperator), typeof(TurretOperatorCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(GlobeHead), typeof(GlobeHeadCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(Axebot), typeof(AxebotCorpse)));
		Tools._monsterToCorpseCard.Add((typeof(TorchHeadAmalgam), typeof(TorchHeadAmalgamCorpse)));
		return true;
	}
	private static void TryRegisterGodotScriptAssembly()
	{
		try
		{
			Assembly modAssembly = typeof(RinInit).Assembly;
			Type? scriptManagerBridgeType = Type.GetType("Godot.Bridge.ScriptManagerBridge, GodotSharp");

			if (scriptManagerBridgeType == null)
			{
				return;
			}

			MethodInfo? lookupMethod = scriptManagerBridgeType.GetMethod(
				"LookupScriptsInAssembly",
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
				binder: null,
				types: [typeof(Assembly)],
				modifiers: null
			);

			lookupMethod ??= scriptManagerBridgeType
				.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
				.FirstOrDefault(m =>
				{
					ParameterInfo[] ps = m.GetParameters();
					return ps.Length == 1
						&& ps[0].ParameterType == typeof(Assembly)
						&& (m.Name.Contains("Lookup", StringComparison.OrdinalIgnoreCase)
							|| m.Name.Contains("Load", StringComparison.OrdinalIgnoreCase)
							|| m.Name.Contains("Register", StringComparison.OrdinalIgnoreCase));
				});

			lookupMethod?.Invoke(null, [modAssembly]);
		}
		catch (Exception e)
		{
			Log.Error($"Failed to register Godot scripts for TH_Rin: {e}");
		}
	}
	}

	[HarmonyPatch]
	public static class ModSfxCmdPatch
	{
		static IEnumerable<MethodBase> TargetMethods()
		{
			return typeof(SfxCmd)
				.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
				.Where(m =>
				{
					if (m.Name != "Play")
					{
						return false;
					}

					ParameterInfo[] ps = m.GetParameters();
					return ps.Length >= 1 && ps[0].ParameterType == typeof(string);
				});
		}

		static bool Prefix(MethodBase __originalMethod, object[] __args)
		{
			return ModSfxPatch.HandlePlay(__originalMethod, __args);
		}
	}

	[HarmonyPatch]
	public static class ModSfxPatch
	{
		private const string ModSfxPrefix = "mod_sfx://";
		private const float DefaultGain = 0.45f;
		private static readonly Dictionary<string, float> GainOverrides = new()
		{
			["TH_Rin/ArtWorks/SFX/characterselect.wav"] = 2.8f,
			["TH_Rin/ArtWorks/SFX/kon.wav"] = 2.8f,
			["TH_Rin/ArtWorks/SFX/cg.wav"] = 3.2f
		};

		static IEnumerable<MethodBase> TargetMethods()
		{
			return typeof(NAudioManager)
				.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
				.Where(m =>
				{
					if (m.Name != "PlayOneShot")
					{
						return false;
					}

					ParameterInfo[] ps = m.GetParameters();
					return ps.Length >= 1 && ps[0].ParameterType == typeof(string);
				});
		}

		static bool Prefix(MethodBase __originalMethod, object[] __args)
		{
			return HandlePlay(__originalMethod, __args);
		}

		public static bool HandlePlay(MethodBase __originalMethod, object[] __args)
		{
			if (__args.Length < 1 || __args[0] is not string path || !path.StartsWith(ModSfxPrefix))
			{
				return true;
			}

			float volume = 1f;
			ParameterInfo[] ps = __originalMethod.GetParameters();
			for (int i = 1; i < __args.Length && i < ps.Length; i++)
			{
				if (__args[i] is float f && ps[i].ParameterType == typeof(float) && ps[i].Name != null && ps[i].Name.Contains("volume", StringComparison.OrdinalIgnoreCase))
				{
					volume = f;
					break;
				}
			}
			if (volume == 1f)
			{
				for (int i = 1; i < __args.Length; i++)
				{
					if (__args[i] is float f)
					{
						volume = f;
					}
				}
			}

			try
			{
				PlayModSfx(path, volume);
			}
			catch (System.Exception e)
			{
				Log.Error($"Failed to play mod sfx: {path}. Error: {e.Message}");
			}

			return false;
		}

		private static void PlayModSfx(string path, float volume)
		{
			string localPath = path.Substring(ModSfxPrefix.Length);
			string resPath = "res://" + localPath;
			AudioStream? stream = ResourceLoader.Load<AudioStream>(resPath);
			if (stream == null)
			{
				return;
			}

			var player = new AudioStreamPlayer();
			player.Stream = stream;
			player.Bus = FindSfxBusName();

			float gain = DefaultGain;
			if (GainOverrides.TryGetValue(localPath, out float overrideGain))
			{
				gain *= overrideGain;
			}
			player.VolumeDb = Mathf.LinearToDb(Mathf.Max(0.0001f, volume * gain));

			if (NGame.Instance != null)
			{
				NGame.Instance.AddChild(player);
			}
			else
			{
				Log.Error($"TH_Rin mod_sfx can't play because NGame.Instance is null. Path: {path}");
				player.QueueFree();
				return;
			}

			player.Play();
			player.Connect("finished", Callable.From(player.QueueFree));
		}

		private static string FindSfxBusName()
		{
			int count = AudioServer.BusCount;
			for (int i = 0; i < count; i++)
			{
				string bus = AudioServer.GetBusName(i);
				if (string.Equals(bus, "SFX", StringComparison.OrdinalIgnoreCase))
				{
					return bus;
				}
			}

			for (int i = 0; i < count; i++)
			{
				string bus = AudioServer.GetBusName(i);
				string lower = bus.ToLowerInvariant();
				if (lower.Contains("sfx") || lower.Contains("soundeffect") || lower.Contains("sound_effect") || lower == "se")
				{
					return bus;
				}
			}

			return "Master";
		}
	}
}

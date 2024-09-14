#if UNITY_2022_3_OR_NEWER
using System.Threading.Tasks;
using MessagePack;
using NUnit.Framework;
using VitalRouter.MRuby;

namespace VitalRouter.Tests
{
    [MessagePackObject(keyAsPropertyName: true)]
    class StateCommand : ICommand
    {
        public bool BoolValue { get; set; }
        public int IntValue { get; set; }
        public string StringValue { get; set; } = default!;
    }

    [MRubyCommand("state", typeof(StateCommand))]
    public partial class TestCommandPreset : MRubyCommandPreset
    {
    }

    [TestFixture]
    public class MRubyScriptTest
    {
        [Test]
        public async Task Subscribers()
        {
            var router = new Router();
            var commandPreset = new TestCommandPreset();
            var ctx = MRubyContext.Create(router, commandPreset);
            ctx.SharedState.SetBool("b", true);
            ctx.SharedState.SetInt("i", 123);
            ctx.SharedState.SetString("s", "hoge moge");

            var script = ctx.CompileScript(
                "cmd :state,\n" +
                "  intValue: state[:i],\n" +
                "  stringValue: state[:s],\n" +
                "  boolValue: state[:b]\n" +
                "");

            var stateCommand = default(StateCommand);
            router.Subscribe<StateCommand>((cmd, _) =>
            {
                stateCommand = cmd;
            });

            await script.RunAsync();

            script.Dispose();
            script.Dispose();
            script.Dispose();

            Assert.That(stateCommand!.BoolValue, Is.True);
            Assert.That(stateCommand!.IntValue, Is.EqualTo(123));
            Assert.That(stateCommand!.StringValue, Is.EqualTo("hoge moge"));
        }
    }
}
#endif
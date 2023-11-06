using System.Collections.Generic;
using System.Linq;
using Osyacat.Ecs.Component.Event;
using Project.CoreDomain.Services.Engine;
using Zenject;

namespace Project.GameDomain.ScreensDomain.BattleDomain.Areas.Ecs.Effect
{
    public class EffectListener : ComponentListener<EffectComponent>
    {
        private readonly List<string> _toClear = new();
        private Dictionary<string, EffectView> _effectById;
        private IEngineService _engineService;
        private List<string> _componentEffects;

        [Inject]
        private void Construct(IEngineService engineService)
        {
            _engineService = engineService;
        }

        public override void OnChanged(EffectComponent component)
        {
            _componentEffects = component.Effects;

            foreach (var id in _componentEffects)
            {
                if (!_effectById[id].IsEffecting())
                {
                    var effect = _effectById[id];
                    effect.PlayEffect();
                }
            }
        }

        private void Awake()
        {
            _effectById = GetComponentsInChildren<EffectView>().ToDictionary(k => k.Id, v => v);
            _engineService.Updating += OnUpdating;
        }

        private void OnDestroy()
        {
            _engineService.Updating -= OnUpdating;
        }

        private void OnUpdating()
        {
            if (_componentEffects != null)
            {
                _toClear.Clear();
                
                if (_componentEffects.Count == 0)
                {
                    _componentEffects = null;
                }
                else
                {
                    foreach (var id in _componentEffects)
                    {
                        if (!_effectById[id].IsEffecting())
                        {
                            _toClear.Add(id);
                        }
                    }
                    
                    foreach (var id in _toClear)
                    {
                        _componentEffects.Remove(id);
                    }
                }
            }
        }
    }
}
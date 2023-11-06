using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Project.CoreDomain.Services.Audio;
using Project.CoreDomain.Services.Audio.Configs.AudioPlayer;
using Project.CoreDomain.Services.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.GameDomain.ScreensDomain.MergeDomain.Areas.Farm.View
{
    public class AnimalView : MonoBehaviour, IAnimalView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteLibrary _spriteLibrary;
        [SerializeField] private Animator _animator;
        [SerializeField] private Animator _mergeAnimator;
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _upWall;
        [SerializeField] private float _waitingToNextTargetSeconds = 2f;
        [SerializeField] private AudioPlayerConfig _dropSound;
        [SerializeField] private AudioPlayerConfig _mergeSound;
        [SerializeField] private AudioPlayerConfig _newCollectionSound;
        [SerializeField] private GameObject _maxGameObject;
        [SerializeField] private GameObject _imageObject;
        [SerializeField] private ShitView _shitPrefab;
        private Vector3 _leftDownCorner;
        private Vector3 _rightUpCorner;
        private Vector3 _target;
        private float _timer;
        private string _animatorState;
        private bool _isDrag;
        private readonly List<int> _contacts = new();
        private IAudioService _audioService;
        private ShitController _shitController;
        private IViewService _viewService;

        public event Action<List<int>> Contacted;

        public int Id { get; set; }

        public bool IsMax
        {
            set => _maxGameObject.SetActive(value);
        }

        public bool IsVisible { set => gameObject.SetActive(value); }

        [Inject]
        private void Constructor(
            IAudioService audioService,
            IViewService viewService
        )
        {
            _audioService = audioService;
            _viewService = viewService;
        }

        private void Awake()
        {
            var leftDownCorner = Camera.main.ScreenToWorldPoint(new Vector3());
            var rightDownCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

            _animator.speed = 0.6f;
            _leftDownCorner = leftDownCorner;
            _rightUpCorner = new Vector3(rightDownCorner.x, _upWall.y, 0);

            InitializePosition();
        }

        private void Start()
        {
            _shitController = new ShitController(Id, _shitPrefab, _viewService, this);
        }

        private void Update()
        {
            _timer -= Time.deltaTime;

            UpdateMove();
            _shitController.Update();
        }

        private void UpdateMove()
        {
            if (_timer < 0)
            {
                UpdateAnimation("Move");
                MoveRandomly();
            }
            else
            {
                UpdateAnimation("Idle");
            }
        }

        private void MoveRandomly()
        {
            var position = transform.position;

            if (Vector3.Distance(_target, position) > 0.1f)
            {
                var heading = _target - position;
                var distance = heading.magnitude;
                var direction = heading / distance;
                transform.position = position + direction * _speed * Time.deltaTime;
            }
            else
            {
                _timer = _waitingToNextTargetSeconds;
                UpdateTarget();
            }
        }

        private void UpdateTarget()
        {
            var randomX = Random.Range(_leftDownCorner.x, _rightUpCorner.x);
            var randomY = Random.Range(_leftDownCorner.y, _rightUpCorner.y);
            _target = new Vector3(randomX, randomY, 0);
            UpdateLookDirection();
        }

        private void InitializePosition()
        {
            UpdateTarget();
            transform.position = _target;
            UpdateTarget();
            var isStaying = Random.Range(0, 2) == 0;
            _timer = isStaying ? _waitingToNextTargetSeconds : 0;
        }

        public void SetSpriteLibrary(SpriteLibraryAsset spriteLibraryAsset)
        {
            _spriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
        }

        private void UpdateAnimation(string state)
        {
            if (_animatorState != state)
            {
                _animatorState = state;
                _animator.Rebind();
                _animator.Update(0f);
                _animator.Play(state);
            }
        }

        private void OnMouseUpAsButton()
        {
            _shitController.Create();
        }

        private void OnMouseDown()
        {
            _isDrag = true && !IsPointerOverUIObject();
        }
        
        private bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        private void OnMouseDrag()
        {
            if (_isDrag)
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.touches.Length > 0 ? (Input.touches[0].position) : Input.mousePosition);
                mousePosition.z = 0;
                if (mousePosition.y > _upWall.y)
                {
                    mousePosition.y = _upWall.y;
                }

                mousePosition.y -= 0.5f;

                transform.position = mousePosition;
            }
        }

        private void OnMouseUp()
        {
            if (_isDrag)
            {
                transform.DOPunchScale(new Vector3(0.2f, -0.4f, 0), 0.5f, 1).OnComplete(() => transform.localScale = Vector3.one);
                _audioService.Sound.PlayImmediately(_dropSound);
                _isDrag = false;
                UpdateLookDirection();
                if (_contacts.Count > 0)
                {
                    Contacted?.Invoke(_contacts);
                }
            }
        }

        private void UpdateLookDirection()
        {
            _imageObject.transform.eulerAngles = transform.position.x > _target.x ? new Vector3(0, 180, 0) : Vector3.zero;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var otherAnimal = other.gameObject.GetComponent<AnimalView>();
            if (otherAnimal != null)
            {
                _contacts.Add(otherAnimal.Id);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            var otherAnimal = other.gameObject.GetComponent<AnimalView>();
            if (otherAnimal != null)
            {
                _contacts.Remove(otherAnimal.Id);
            }
        }

        public void MergeEffect()
        {
            _mergeAnimator.Play("Merge");
            _audioService.Sound.PlayImmediately(_mergeSound);
        }

        public async UniTask PlayNewCollectionEffect()
        {
            await UniTask.Delay(300);
            UpdateAnimation("Idle");
            _timer = 10000;
            _spriteRenderer.sortingLayerName = "TopTop";
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            _audioService.Sound.PlayImmediately(_newCollectionSound);
            worldPosition.z = 0;
            transform.DOMove(worldPosition, 2f);
            await UniTask.Delay(7000);
            _timer = 0;
            UpdateTarget();
            _spriteRenderer.sortingLayerName = "Default";
        }
    }
}
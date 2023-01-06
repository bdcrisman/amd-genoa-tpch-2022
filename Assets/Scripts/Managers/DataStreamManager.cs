using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataStreamManager : MonoBehaviour {
    public bool IsPathsDrawn { get; private set; }

    [Header("Stream spawner parents")]
    [SerializeField] private Transform _leftProcessorStreamsParent;
    [SerializeField] private Transform _rightProcessorStreamsParent;

    [Header("Stream spawners")]
    [SerializeField] private StreamSpawner _leftDatabaseStreamSpawner;
    [SerializeField] private StreamSpawner _rightDatabaseStreamSpawner;
    [SerializeField] private StreamSpawner _leftScoreStreamSpawner;
    [SerializeField] private StreamSpawner _rightScoreStreamSpawner;

    [Header("Paths to be drawn")]
    [SerializeField] private List<Image> _procsToDatabase;
    [SerializeField] private List<Image> _databaseToProcs;
    [SerializeField] private List<Image> _procsToScores;

    [Header("Query object")]
    [SerializeField] private QueryItem _queryItemPrefab;
    [SerializeField] private List<Transform> _querySpawnPositions;

    [Header("Colors")]
    [SerializeField] private Color _mainBackgroundColor;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _glowColor;
    [SerializeField] private Color _questionMarkColor;

    private List<StreamSpawner> _leftProcessorStreamSpawners;
    private List<StreamSpawner> _rightProcessorStreamSpawners;
    private bool _isRunning;

    private void Start() {
        foreach (var path in _procsToDatabase) {
            path.fillAmount = 0f;
        }

        foreach (var path in _databaseToProcs) {
            path.fillAmount = 0f;
        }

        foreach (var path in _procsToScores) {
            path.fillAmount = 0f;
        }
    }

    private void OnDisable() {
        _leftProcessorStreamSpawners.ForEach(x => x.Finished -= (s, e) => OnProcessorSpawnerFinished(true));
        _rightProcessorStreamSpawners.ForEach(x => x.Finished -= (s, e) => OnProcessorSpawnerFinished(false));

        _leftDatabaseStreamSpawner.Finished -= (s, e) => OnDatabaseSpawnerFinished();
        _rightDatabaseStreamSpawner.Finished -= (s, e) => OnDatabaseSpawnerFinished();
    }

    private void OnDatabaseSpawnerFinished() {
        _leftScoreStreamSpawner.Run();
        _rightScoreStreamSpawner.Run();
    }

    private void OnProcessorSpawnerFinished(bool isLeftSide) {
        _leftDatabaseStreamSpawner.Run();
        _rightDatabaseStreamSpawner.Run();

        // query object
        SpawnQueryObject(isLeftSide);
    }

    public void Setup(float delta) {
        // get proc spawners
        _leftProcessorStreamSpawners = _leftProcessorStreamsParent.GetComponentsInChildren<StreamSpawner>().ToList();
        _rightProcessorStreamSpawners = _rightProcessorStreamsParent.GetComponentsInChildren<StreamSpawner>().ToList();

        // events
        _leftProcessorStreamSpawners.ForEach(x => x.Finished += (s, e) => OnProcessorSpawnerFinished(true));
        _rightProcessorStreamSpawners.ForEach(x => x.Finished += (s, e) => OnProcessorSpawnerFinished(false));
        _leftDatabaseStreamSpawner.Finished += (s, e) => OnDatabaseSpawnerFinished();
        _rightDatabaseStreamSpawner.Finished += (s, e) => OnDatabaseSpawnerFinished();

        // setup
        _leftProcessorStreamSpawners.ForEach(x => x.Setup(delta));
        _rightProcessorStreamSpawners.ForEach(x => x.Setup(delta));
        _leftDatabaseStreamSpawner.Setup(delta);
        _rightDatabaseStreamSpawner.Setup(delta);
        _leftScoreStreamSpawner.Setup(delta);
        _rightScoreStreamSpawner.Setup(delta);
    }

    public IEnumerator SetupDemoCo() {
        yield return StartCoroutine(DrawPathsCo());
        IsPathsDrawn = true;
    }

    public void Run() {
        if (_isRunning) return;
        _isRunning = true;

        _leftProcessorStreamSpawners.ForEach(x => x.Run());
        _rightProcessorStreamSpawners.ForEach(x => x.Run());
    }

    public void Stop() {
        _leftProcessorStreamSpawners.ForEach(x => x.Stop());
        _rightProcessorStreamSpawners.ForEach(x => x.Stop());

        _leftDatabaseStreamSpawner.Stop();
        _rightDatabaseStreamSpawner.Stop();

        _leftScoreStreamSpawner.Stop();
        _rightScoreStreamSpawner.Stop();
    }

    private IEnumerator DrawPathsCo() {
        var duration = 0.25f;
        yield return StartCoroutine(DrawListPathsCo(_procsToDatabase, duration));
        yield return StartCoroutine(DrawListPathsCo(_databaseToProcs, duration));
        yield return StartCoroutine(DrawListPathsCo(_procsToScores, duration));
    }

    private IEnumerator DrawListPathsCo(List<Image> paths, float duration) {
        for (var i = 0; i < paths.Count; i += 2) {
            var path0 = paths[i];
            var path1 = paths[i + 1];
            var t = 0f;

            while (t < duration) {
                var fillAmount = Mathf.Lerp(0, 1, t / duration);
                path0.fillAmount = fillAmount;
                path1.fillAmount = fillAmount;
                t += Time.deltaTime;
                yield return null;
            }
            path0.fillAmount = 1;
            path1.fillAmount = 1;
        }
    }

    private void SpawnQueryObject(bool isLeftSide) {
        if (isLeftSide) {
            var queryItem_0 = Instantiate(_queryItemPrefab, _querySpawnPositions[0]);
            queryItem_0.Setup(gameObject.layer, _querySpawnPositions[0].position, GetRandQueryType(), new QueryItemColors {
                MainBackgroundColor = _mainBackgroundColor,
                BaseColor = _baseColor,
                GlowColor = _glowColor,
                QuestionMarkColor = _questionMarkColor
            });

            return;
        }
        
        // right side
        var queryItem_1 = Instantiate(_queryItemPrefab, _querySpawnPositions[1]);
        queryItem_1.Setup(gameObject.layer, _querySpawnPositions[1].position, GetRandQueryType(), new QueryItemColors {
            MainBackgroundColor = _mainBackgroundColor,
            BaseColor = _baseColor,
            GlowColor = _glowColor,
            QuestionMarkColor = _questionMarkColor
        });
    }

    private QueryType GetRandQueryType() {
        var rand = UnityEngine.Random.Range(0, 4);
        return (QueryType)rand;
    }
}

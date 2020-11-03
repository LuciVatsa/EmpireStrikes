using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class FireBullets : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject bullet;

    [Header("Data")]
    public float launchVelocity = 10000;

    [Header("Recoil")]
    public TextAsset recoilFormat;
    public float recoilOffset;
    public float recoilResetTime;

    private List<RecoilOffset> _recoilOffsets;
    private Dictionary<int, Dictionary<int, char>> _recoilMatrix;
    private float _currentRecoilTime;
    private int _currentRecoilIndex;

    #region Unity Functions

    private void Start()
    {
        _recoilOffsets = new List<RecoilOffset>();
        _recoilMatrix = new Dictionary<int, Dictionary<int, char>>();

        _currentRecoilTime = 0;
        _currentRecoilIndex = 0;

        SaveRecoilOffsets(recoilFormat.text);
    }

    private void Update()
    {
        if (_currentRecoilTime > 0)
        {
            _currentRecoilTime -= Time.deltaTime;

            if (_currentRecoilTime <= 0)
            {
                _currentRecoilIndex = 0;
            }
        }
    }

    #endregion

    #region External Functions

    public void Fire()
    {
        int recoilIndex = _currentRecoilIndex;

        _currentRecoilIndex += 1;
        if (_currentRecoilIndex >= _recoilOffsets.Count)
        {
            _currentRecoilIndex = _recoilOffsets.Count - 1;
        }

        Vector2 offset = _recoilOffsets[recoilIndex].offset;
        Vector3 direction = transform.forward + new Vector3(offset.x * recoilOffset, offset.y * recoilOffset, 0);
        _currentRecoilTime = recoilResetTime;

        GameObject shot = GameObject.Instantiate(bullet, transform.position, transform.rotation);
        shot.gameObject.GetComponent<Rigidbody>().AddForce(direction * launchVelocity);

        Destroy(shot, 2);
    }

    #endregion

    #region Utility Functions

    private void SaveRecoilOffsets(string recoilText)
    {
        int currentRowIndex = 0;
        int currentColumnIndex = 0;

        int maxColumns = 0;

        for (int i = 0; i < recoilText.Length; i++)
        {
            var letter = recoilText[i];

            if (!_recoilMatrix.ContainsKey(currentColumnIndex))
            {
                Dictionary<int, char> dict = new Dictionary<int, char>();
                _recoilMatrix.Add(currentRowIndex, dict);
            }

            if (letter == '\r')
            {
                currentRowIndex += 1;
                currentColumnIndex = 0;

                i += 1;
            }
            else if (letter == '\n')
            {
                currentRowIndex += 1;
                currentColumnIndex = 0;
            }
            else
            {
                if (maxColumns < currentColumnIndex)
                {
                    maxColumns = currentColumnIndex;
                }

                bool numberCastSuccess = int.TryParse(letter.ToString(), out int result);
                if (numberCastSuccess)
                {
                    _recoilOffsets.Add(new RecoilOffset()
                    {
                        index = result,
                        rowIndex = currentRowIndex,
                        columnIndex = currentColumnIndex,
                        offset = Vector2.zero
                    });

                    _recoilMatrix[currentRowIndex].Add(currentColumnIndex, letter);
                }

                currentColumnIndex += 1;
            }
        }

        int centerRow = currentRowIndex / 2;
        int centerColumn = currentColumnIndex / 2;

        for (int i = 0; i < _recoilOffsets.Count; i++)
        {
            var recoilData = _recoilOffsets[i];

            int rowDiff = centerRow - recoilData.rowIndex;
            int columnDiff = centerColumn - recoilData.columnIndex;

            Vector2 offset = new Vector2(columnDiff, rowDiff);
            recoilData.offset = offset;

            _recoilOffsets[i] = recoilData;
        }

        _recoilOffsets.Sort();
    }

    #endregion

    #region Structs

    public struct RecoilOffset : IComparable<RecoilOffset>
    {
        public int index;

        public int rowIndex;
        public int columnIndex;

        public Vector2 offset;

        public int CompareTo(RecoilOffset other)
        {
            if (index < other.index)
            {
                return -1;
            }

            if (index > other.index)
            {
                return 1;
            }

            return 0;
        }
    }

    #endregion
}

/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using System;
using UnityEngine;
using OVRTouchSample;

namespace OculusSampleFramework
{
    public class DistanceGrabbable : OVRGrabbable
    {
        public string m_materialColorField;


        GrabbableCrosshair m_crosshair;
        GrabManager m_crosshairManager;
        Renderer m_renderer;
        MaterialPropertyBlock m_mpb;


        public bool InRange
        {
            get { return m_inRange; }
            set
            {
                m_inRange = value;
                RefreshCrosshair();
            }
        }
        bool m_inRange;

        public bool Targeted
        {
            get { return m_targeted; }
            set
            {
                m_targeted = value;
                RefreshCrosshair();
            }
        }
        bool m_targeted;


        // 추가된 코드
        private StuckObject studkObj;
        public StuckObject StuckObj
        {
            get { return studkObj; }
        }
        private float mass;
        public float Mass
        {
            get { return mass; }
        }

        private ThrowGrabbed throwGrabbed;

        protected override void Start()
        {
            base.Start();

            // ThrowGrabbed 스크립트 자동 추가
            throwGrabbed = GetComponent<ThrowGrabbed>();
            if (throwGrabbed == null)
                throwGrabbed = gameObject.AddComponent<ThrowGrabbed>();

            // 힘과 무게 비교 처리를 위해 추가
            mass = GetComponent<Rigidbody>().mass;
            // 오브젝트 부착 처리를 위해 추가
            studkObj = GetComponent<StuckObject>();

            m_crosshair = gameObject.GetComponentInChildren<GrabbableCrosshair>();
            m_renderer = gameObject.GetComponent<Renderer>();
            m_crosshairManager = FindObjectOfType<GrabManager>();
            m_mpb = new MaterialPropertyBlock();
            //RefreshCrosshair();
            m_mpb.SetColor(m_materialColorField, Color.white);
            m_renderer.SetPropertyBlock(m_mpb);
        }

        void RefreshCrosshair()
        {
            if (m_crosshair)
            {
                if (isGrabbed) m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
                else if (!InRange) m_crosshair.SetState(GrabbableCrosshair.CrosshairState.Disabled);
                else m_crosshair.SetState(Targeted ? GrabbableCrosshair.CrosshairState.Targeted : GrabbableCrosshair.CrosshairState.Enabled);
            }
            if (m_materialColorField != null)
            {
                m_renderer.GetPropertyBlock(m_mpb);
                if (isGrabbed || !InRange) m_mpb.SetColor(m_materialColorField, Color.white);
                else if (Targeted) m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorHighlighted);
                else m_mpb.SetColor(m_materialColorField, m_crosshairManager.OutlineColorInRange);
                m_renderer.SetPropertyBlock(m_mpb);
            }
        }

        public void SetColor(Color focusColor)
        {
            m_mpb.SetColor(m_materialColorField, focusColor);
            m_renderer.SetPropertyBlock(m_mpb);
        }

        public void ClearColor()
        {
            m_mpb.SetColor(m_materialColorField, Color.white);
            m_renderer.SetPropertyBlock(m_mpb);
        }
    }
}

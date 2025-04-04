﻿using System.Collections.Generic;
using UnityEngine;

namespace PaladinMod.Modules {
    internal static class Materials {

        private static List<Material> cachedMaterials = new List<Material>();

        public static Shader hotpoo = RoR2.LegacyResourcesAPI.Load<Shader>("Shaders/Deferred/HGStandard");

        public static Material CreateHotpooMaterial(string materialName) {
            Material tempMat = cachedMaterials.Find(mat => {
                materialName.Replace(" (Instance)", "");
                return mat.name.Contains(materialName); 
            });
            if (tempMat) {
                return tempMat;
            }

            //Material mat = UnityEngine.Object.Instantiate<Material>(Assets.commandoMat);
            tempMat = Asset.mainAssetBundle.LoadAsset<Material>(materialName);

            if (!tempMat) {
                Debug.LogError("Failed to load material: " + materialName + " - Check to see that the name in your Unity project matches the one in this code");
                return new Material(hotpoo);
            }

            return tempMat.SetHotpooMaterial();
        }

        private static Material CreateHotpooMaterial(Material tempMat) {
            if (cachedMaterials.Contains(tempMat)) {
                return tempMat;
            }           
            return tempMat.SetHotpooMaterial();
        }
        
        public static Material SetHotpooMaterial(this Material tempMat) {
            #region checks
            if (cachedMaterials.Contains(tempMat)) {
                return tempMat;
            }

            if(tempMat.shader.name == "StubbedShader/deferred/hgstandard") {
                tempMat.shader = hotpoo;
                return tempMat;
            }
            #endregion

            float? bumpScale = null;
            Color? emissionColor = null;

            //grab values before the shader changes
            if (tempMat.IsKeywordEnabled("_NORMALMAP")) {
                bumpScale = tempMat.GetFloat("_BumpScale");
            }
            if (tempMat.IsKeywordEnabled("_EMISSION")) {
                emissionColor = tempMat.GetColor("_EmissionColor");
            }

            tempMat.shader = hotpoo;

            //apply values after shader is set
            tempMat.SetColor("_Color", tempMat.GetColor("_Color"));
            tempMat.SetTexture("_MainTex", tempMat.GetTexture("_MainTex"));
            tempMat.SetTexture("_EmTex", tempMat.GetTexture("_EmissionMap"));

            if (bumpScale != null) {
                tempMat.SetFloat("_NormalStrength", (float)bumpScale);
            }
            if (emissionColor != null) {
                tempMat.SetColor("_EmColor", (Color)emissionColor);
                tempMat.SetFloat("_EmPower", 1);
            }

            if(tempMat.IsKeywordEnabled("_CULL")) {
                tempMat.SetInt("_Cull", 0);
            }

            tempMat.EnableKeyword("DITHER");

            //set this keyword in unity if you've set up your model for limb removal item displays (eg. goat hoof) by setting your model's vertex colors
            if (tempMat.IsKeywordEnabled("LIMBREMOVAL")) {
                tempMat.SetInt("_LimbRemovalOn", 1);
            }

            cachedMaterials.Add(tempMat);
            return tempMat;
        }

        /// <summary>
        /// makes this a unique material if we already have this material cached (i.e. you want a changed variant). New material will not be cached
        /// If it was not cached in the first place, simply returns as it is unique.
        /// </summary>
        public static Material MakeUnique(this Material material) {

            if (cachedMaterials.Contains(material)) {

                return new Material(material);
            }
            return material;
        }

        public static Material SetColor(this Material material, Color color) {
            material.SetColor("_Color", color);
            return material;
        }

        public static Material SetNormal(this Material material, float normalStrength = 1) {
            material.SetFloat("_NormalStrength", normalStrength);
            return material;
        }
        
        public static Material SetEmission(this Material material) => SetEmission(material, 1);
        public static Material SetEmission(this Material material, float emission) {
            material.SetFloat("_EmPower", emission);
            return material;
        }
        public static Material SetEmission(this Material material, float emission, Color emissionColor) {
            material.SetFloat("_EmPower", emission);
            material.SetColor("_EmColor", emissionColor);
            return material;
        }
        public static Material SetCull(this Material material, bool cull = false) {
            material.SetInt("_Cull", cull ? 1 : 0);
            return material;
        }
    }
}
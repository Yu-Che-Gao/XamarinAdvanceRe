﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System.IO;
using DrawFunction;

namespace XamarinAdvanceRe.Services
{
    class EmotionService
    {
        EmotionServiceClient emotionserviceclient;

        public EmotionService()
        {
            emotionserviceclient = new EmotionServiceClient(Constant.EmotionApiKey);
        }

        public async Task<MyDataType> DetectEmotion()
        {
            Emotion[] emotionResult;

            try
            {
                emotionResult = await emotionserviceclient.RecognizeAsync(Constant.ImageUri.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            MyDataType mydata = new MyDataType();
            mydata.rects = new List<Rect>();
            mydata.emoes = new List<string>();
            foreach (var item in emotionResult)
            {
                mydata.rects.Add(new Rect(item.FaceRectangle.Left, item.FaceRectangle.Top, item.FaceRectangle.Height));
                mydata.emoes.Add(item.Scores.ToRankedList().ToList()[0].Key);
            }

            return mydata;
        }

        public async Task<List<KeyValuePair<string, float>>> GetEmotionRankAsync(Stream imageStream)
        {
            var emotionResult = await emotionserviceclient.RecognizeAsync(imageStream);
            return emotionResult[0].Scores.ToRankedList().ToList();
        }
    }
}

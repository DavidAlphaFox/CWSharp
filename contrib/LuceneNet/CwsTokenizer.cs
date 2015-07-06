// Copyright (c) Yamool. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Lucene.Net.Analysis.CWSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Lucene.Net.Analysis;
    using Lucene.Net.Analysis.Tokenattributes;
    using Token2 = Yamool.CWSharp.Token;

    internal sealed class CwsTokenizer : Tokenizer
    {
        private ITermAttribute _termAtt;
        private IOffsetAttribute _offsetAtt;
        private ITypeAttribute _typeAtt;
        private int _offset;

        private IEnumerator<Token2> _iteration;       

        private CwsTokenizer() { }

        public CwsTokenizer(IEnumerator<Token2> iteration)
        {
            _iteration = iteration;
            this.Init();
        }

        public override bool IncrementToken()
        {
            if (!_iteration.MoveNext())
            {
                return false;
            }
            var tokenResult = _iteration.Current;
            _termAtt.SetTermBuffer(tokenResult.Text);            
            _typeAtt.Type = (string)tokenResult.Type;
            _offsetAtt.SetOffset(_offset, _offset + tokenResult.Length);
            _offset += tokenResult.Length;
            return true;
        }

        private void Init()
        {
            _termAtt = this.AddAttribute<ITermAttribute>();
            _offsetAtt = this.AddAttribute<IOffsetAttribute>();
            _typeAtt = this.AddAttribute<ITypeAttribute>();
        }
    }
}

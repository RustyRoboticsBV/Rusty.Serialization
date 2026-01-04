# Supported Format Table

Below you can view the how the different nodes are encoded in CSCD, JSON and XML.

Note that not all strings of valid JSON and XML can be interpreted by the parser. 

|Node|CSCD|JSON|XML|
|-|-|-|-|
|Null|null|null|&lt;null/&gt;|
|Bool|true|true|&lt;bool&gt;true&lt;/bool&gt;|
|Int|123|123|&lt;int&gt;123&lt;/int&gt;|
|Real|1.0|1.0|&lt;real&gt;1.0&lt;/real&gt;|
|Char|'A'|"A"|&lt;char&gt;A&lt;/char&gt;|
|String|"abc"|"abc"|&lt;str&gt;abc&lt;/str&gt;|
|Color|#FF0000FF|"FF0000FF"|&lt;color&gt;FF0000FF&lt;/color&gt;|
|Time|Y1999M2D13h10m5s2f111|"1999-2-13,10:5:2.111"|&lt;time&gt;<br>&nbsp;&nbsp;&lt;year&gt;1999&lt;/year&gt; <br>&nbsp;&nbsp;&lt;month&gt;2&lt;/month&gt; <br>&nbsp;&nbsp;&lt;day&gt;13&lt;/day&gt; <br>&nbsp;&nbsp;&lt;hour&gt;10&lt;/hour&gt; <br>&nbsp;&nbsp;&lt;minute&gt;5&lt;/minute&gt; <br>&nbsp;&nbsp;&lt;second&gt;2&lt;/second&gt; <br>&nbsp;&nbsp;&lt;millisecond&gt;111&lt;/millisecond&gt; <br>&lt;/time&gt;|
|Bytes|b1234ABCD|"123ABCD"|&lt;bytes&gt;1234ABCD&lt;/bytes&gt;|
|Ref|&my_ref|{"$ref": "my_ref"}|&lt;ref&gt;my_ref&lt;/ref&gt;|
|List|[1,2,3]|[1, 2, 3]|&lt;list&gt;<br>&nbsp;&nbsp;&lt;int&gt;1&lt;/int&gt;<br>&nbsp;&nbsp;&lt;int&gt;2&lt;/int&gt;<br>&nbsp;&nbsp;&lt;int&gt;3&lt;/int&gt;<br>&lt;/list&gt;|
|Dict|{123:true,'A':1.0,"abc":null}|{"$dict": [<br>&nbsp;&nbsp;{"key": 123, "value": true}, <br>&nbsp;&nbsp;{"key": "A", "value": 1.0}, <br>&nbsp;&nbsp;{"key": "abc", "value": null}<br>]}|&lt;dict&gt;<br>&nbsp;&nbsp;&lt;entry&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;int&gt;123&lt;/int&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;bool&gt;true&lt;/bool&gt;<br>&nbsp;&nbsp;&lt;/entry&gt;<br>&nbsp;&nbsp;&lt;entry&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;char&gt;A&lt;/char&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;real&gt;1.0&lt;/real&gt;<br>&nbsp;&nbsp;&lt;/entry&gt;<br>&nbsp;&nbsp;&lt;entry&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;str&gt;abc&lt;/str&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;null/&gt;<br>&nbsp;&nbsp;&lt;/entry&gt;<br>&lt;/dict&gt;|
|Object|<a:0,b:"abc",c:null>|{<br>&nbsp;&nbsp;"a": 0, <br>&nbsp;&nbsp;"b": "abc", <br>&nbsp;&nbsp;"c": null<br>}|&lt;obj&gt;<br>&nbsp;&nbsp;&lt;a&gt;&lt;bool&gt;0&lt;/bool&gt;&lt;/a&gt;<br>&nbsp;&nbsp;&lt;b&gt;&lt;real&gt;abc&lt;/real&gt;&lt;/b&gt;<br>&nbsp;&nbsp;&lt;c&gt;&lt;null/&gt;&lt;/c&gt;<br>&lt;/obj&gt;|
|Type|(my_type)...|{"$type": "my_type", "$value": ... }|&lt;type name="my_type"&gt;...&lt;/type&gt;|
|ID|&#96;my_id&#96;...|{"$id": "my_id", "$value": ... }|&lt;id name="my_id"&gt;...&lt;/id&gt;|

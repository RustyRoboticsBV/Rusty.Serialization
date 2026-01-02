# Supported Format Table

Below you can view the how the different nodes are encoded in CSCD, JSON and XML.

Note that not all strings of valid JSON and XML can be interpreted by the parser. 

|Node|CSCD|JSON|XML|
|-|-|-|-|
|Null|null|null|&lt;null/&gt;|
|Bool|true|["bool", true]|&lt;bool&gt;true&lt;/bool&gt;|
|Int|123|["int", 123]|&lt;int&gt;123&lt;/int&gt;|
|Real|1.0|["real", 1.0]|&lt;real&gt;1.0&lt;/real&gt;|
|Char|'A'|["char", "A"]|&lt;char&gt;A&lt;/char&gt;|
|String|"abc"|["str", "abc"]|&lt;str&gt;abc&lt;/str&gt;|
|Color|#FF0000FF|["color", "FF0000FF"]|&lt;color&gt;FF0000FF&lt;/color&gt;|
|Time|Y1999M2D13h10m5s2f111|["time", { <br>&nbsp;&nbsp;"year": 1999, <br>&nbsp;&nbsp;"month": 2, <br>&nbsp;&nbsp;"day": 13, <br>&nbsp;&nbsp;"hour": 10, <br>&nbsp;&nbsp;"minute": 5, <br>&nbsp;&nbsp;"second": 2, <br>&nbsp;&nbsp;"millisecond": 111<br>}]|&lt;time&gt;<br>&nbsp;&nbsp;&lt;year&gt;1999&lt;/year&gt; <br>&nbsp;&nbsp;&lt;month&gt;2&lt;/month&gt; <br>&nbsp;&nbsp;&lt;day&gt;13&lt;/day&gt; <br>&nbsp;&nbsp;&lt;hour&gt;10&lt;/hour&gt; <br>&nbsp;&nbsp;&lt;minute&gt;5&lt;/minute&gt; <br>&nbsp;&nbsp;&lt;second&gt;2&lt;/second&gt; <br>&nbsp;&nbsp;&lt;millisecond&gt;111&lt;/millisecond&gt; <br>&lt;/time&gt;|
|Bytes|b1234ABCD|["bytes", "123ABCD"]|&lt;bytes&gt;1234ABCD&lt;/bytes&gt;|
|Ref|&my_ref|["ref", "my_ref"]|&lt;ref&gt;my_ref&lt;/ref&gt;|
|List|[1,2,3]|["list", [<br>&nbsp;&nbsp;["int", 1], <br>&nbsp;&nbsp;["int", 2], <br>&nbsp;&nbsp;["int", 3]<br>]]|&lt;list&gt;<br>&nbsp;&nbsp;&lt;int&gt;1&lt;/int&gt;<br>&nbsp;&nbsp;&lt;int&gt;2&lt;/int&gt;<br>&nbsp;&nbsp;&lt;int&gt;3&lt;/int&gt;<br>&lt;/list&gt;|
|Dict|{123:true,'A':1.0,"abc":null}|["dict", [<br>&nbsp;&nbsp;[["int", 123], true], <br>&nbsp;&nbsp;[["char", "A"], ["real", 1.0]], <br>&nbsp;&nbsp;[["str", "abc"], null]<br>]]|&lt;dict&gt;<br>&nbsp;&nbsp;&lt;entry&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;int&gt;123&lt;/int&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;bool&gt;true&lt;/bool&gt;<br>&nbsp;&nbsp;&lt;/entry&gt;<br>&nbsp;&nbsp;&lt;entry&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;char&gt;A&lt;/char&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;real&gt;1.0&lt;/real&gt;<br>&nbsp;&nbsp;&lt;/entry&gt;<br>&nbsp;&nbsp;&lt;entry&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;str&gt;abc&lt;/str&gt;<br>&nbsp;&nbsp;&nbsp;&nbsp;&lt;null/&gt;<br>&nbsp;&nbsp;&lt;/entry&gt;<br>&lt;/dict&gt;|
|Object|<a:0,b:"abc",c:null>|["obj", {<br>&nbsp;&nbsp;"a": true, <br>&nbsp;&nbsp;"b": ["real", 1.0], <br>&nbsp;&nbsp;"c": null<br>}]|&lt;obj&gt;<br>&nbsp;&nbsp;&lt;a&gt;&lt;bool&gt;true&lt;/bool&gt;&lt;/a&gt;<br>&nbsp;&nbsp;&lt;b&gt;&lt;real&gt;1.0&lt;/real&gt;&lt;/b&gt;<br>&nbsp;&nbsp;&lt;c&gt;&lt;null/&gt;&lt;/c&gt;<br>&lt;/obj&gt;|
|Type|(my_type)<...>|["type", "my_type", [...]]|&lt;obj type="my_type"&gt;...&lt;/obj&gt;|
|ID|&#96;my_id&#96;<...>|["id", "my_id", [...]]|&lt;obj id="my_id"&gt;...&lt;/obj&gt;|

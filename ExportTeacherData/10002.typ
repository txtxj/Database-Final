#set page(numbering: "1", number-align: center)
#set text(font: ("TENGXZTB", "Microsoft Sans Serif"), lang: "zh", size: 12pt)

#let iblue = rgb("4183FF")

#align(center)[
  = 教师教学科研工作统计 1999-2025
]

#pad(top: 10pt)[]

== #text(iblue)[教师基本信息]

#line(length: 100%, stroke: 0.5pt + iblue)

#columns(4)[工号：10002 #colbreak() 姓名：安虹 #colbreak() 性别：女 #colbreak() 职称：教授]

== #text(iblue)[教学情况]

#line(length: 100%, stroke: 0.5pt + iblue)

#{
  let cid = ("CS1234", "CS1235", "CS6666", );
  let cname = ("ICS A", "ICS B", "软件工程", );
  let ccredit = ("70", "60", "60", );
  let cyear = ("1999", "1999", "2021", );
  let cterm = ("秋季学期", "秋季学期", "夏季学期", );
  let num = cid.len();
  if (num == 0) {return;}
  let i = 0;
  grid(columns: (auto, auto, auto, auto), gutter: 10pt,
  ..(while i < num {
    ([课程号：#cid.at(i)],
    [课程名：#cname.at(i)],
    [主讲学时：#ccredit.at(i)],
    [学期：#cyear.at(i) #cterm.at(i)])
    i += 1;
  }))
}

== #text(iblue)[发表论文情况]

#line(length: 100%, stroke: 0.5pt + iblue)

#{
  let paname = ("第1篇论文", "第二篇论文", "你说得对", );
  let pasource = ("花花公子", "花花公子", "IEEE", );
  let payear = ("2022", "1999", "2023", );
  let palevel = ("CCF-A", "CCF-B", "CCF-A", );
  let parank = ("2", "1", "2", );
  let paauthor = ("False", "False", "True", );
  let num = paname.len();
  if (num == 0) {return;}
  let i = 0;
  while i < num {
    [+ #paname.at(i)，#pasource.at(i)，#payear.at(i)，#palevel.at(i)，排名第 #parank.at(i) #{if (paauthor.at(i) == "1") {[，通讯作者]}}]
    i +=1;
  }
}

== #text(iblue)[承担项目情况]

#line(length: 100%, stroke: 0.5pt + iblue)

#{
  let prname = ("东方", );
  let prsource = ("Alice", );
  let prtype = ("省部级项目", );
  let prstart = ("2002", );
  let prend = ("9999", );
  let prfund = ("999", );
  let prfunds = ("1000", );
  let num = prname.len();
  if (num == 0) {return;}
  let i = 0;
  while i < num {
    [+ #prname.at(i)，#prsource.at(i)，#prtype.at(i)，#prstart.at(i)-#prend.at(i)，总经费：#prfunds.at(i)，承担经费：#prfund.at(i)]
    i +=1;
  }
}
#set page(numbering: "1", number-align: center)
#set text(font: ("TENGXZTB", "Microsoft Sans Serif"), lang: "zh", size: 12pt)

#let iblue = rgb("4183FF")

#align(center)[
  = 教师教学科研工作统计 2020-2023
]

#pad(top: 10pt)[]

== #text(iblue)[教师基本信息]

#line(length: 100%, stroke: 0.5pt + iblue)

#columns(4)[工号：10001 #colbreak() 姓名：Citrine #colbreak() 性别：男 #colbreak() 职称：博士后]

== #text(iblue)[教学情况]

#line(length: 100%, stroke: 0.5pt + iblue)

#{
  let cid = ("MATH1001", );
  let cname = ("数学分析 1", );
  let ccredit = ("20", );
  let cyear = ("2020", );
  let cterm = ("春季学期", );
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
  let paname = ("第1篇论文", "你说得对", "论OP的自我修养", );
  let pasource = ("花花公子", "IEEE", "米游社", );
  let payear = ("2022", "2023", "2023", );
  let palevel = ("CCF-A", "CCF-A", "CCF-C", );
  let parank = ("1", "1", "1", );
  let paauthor = ("True", "False", "True", );
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
  let prname = ("X", "东方", );
  let prsource = ("Mihoyo", "Alice", );
  let prtype = ("国家级项目", "省部级项目", );
  let prstart = ("2019", "2002", );
  let prend = ("2024", "9999", );
  let prfund = ("1234", "1", );
  let prfunds = ("5000", "1000", );
  let num = prname.len();
  if (num == 0) {return;}
  let i = 0;
  while i < num {
    [+ #prname.at(i)，#prsource.at(i)，#prtype.at(i)，#prstart.at(i)-#prend.at(i)，总经费：#prfunds.at(i)，承担经费：#prfund.at(i)]
    i +=1;
  }
}
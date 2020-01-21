using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TenderLibrary;

namespace ZcrlTender
{
    // TODO: Окно предложения переноса источников финансирования с прошлого года
    public class TenderDbInitializer : CreateDatabaseIfNotExists<ZcrlTenderContext>
    {
        protected override void Seed(ZcrlTenderContext context)
        {
            UserSession.CreateUser("Адміністратор", "1234567", "Обліковий запис адміністратора системи");

            KekvCode k1 = new KekvCode { Code = "2110", Name = "Оплата праці" };
            KekvCode k2 = new KekvCode { Code = "2210", Name = "Предмети, матеріали, обладнання та інвентар" };
            KekvCode k3 = new KekvCode { Code = "2220", Name = "Медикаменти та перев'язувальні матеріали" };
            KekvCode k4 = new KekvCode { Code = "2230", Name = "Продукти харчування" };
            KekvCode k5 = new KekvCode { Code = "2240", Name = "Оплата послуг (крім комунальних)" };
            KekvCode k6 = new KekvCode { Code = "2271", Name = "Оплата теплопостачання" };
            KekvCode k7 = new KekvCode { Code = "2272", Name = "Оплата водопостачання та водовідведення" };
            KekvCode k8 = new KekvCode { Code = "2273", Name = "Оплата електроенергії" };
            KekvCode k9 = new KekvCode { Code = "2274", Name = "Оплата природного газу" };
            KekvCode k10 = new KekvCode { Code = "2275", Name = "Оплата інших енергоносіїв" };
            KekvCode k11 = new KekvCode { Code = "2281", Name = "Дослідження і розробки, окремі заходи розвитку по реалізації державних (регіональних) програм" };
            KekvCode k12 = new KekvCode { Code = "2282", Name = "Окремі заходи по реалізації державних (регіональних) програм, не віднесені до заходів розвитку" };
            KekvCode k13 = new KekvCode { Code = "2710", Name = "Виплата пенсій і допомоги" };
            KekvCode k14 = new KekvCode { Code = "2730", Name = "Інші виплати населенню" };
            KekvCode k15 = new KekvCode { Code = "2800", Name = "Інші поточні видатки" };
            KekvCode k16 = new KekvCode { Code = "3110", Name = "Придбання обладнання і предметів довгострокового користування" };

            context.KekvCodes.Add(k1);
            context.KekvCodes.Add(k2);
            context.KekvCodes.Add(k3);
            context.KekvCodes.Add(k4);
            context.KekvCodes.Add(k5);
            context.KekvCodes.Add(k6);
            context.KekvCodes.Add(k7);
            context.KekvCodes.Add(k8);
            context.KekvCodes.Add(k9);
            context.KekvCodes.Add(k10);
            context.KekvCodes.Add(k11);
            context.KekvCodes.Add(k12);
            context.KekvCodes.Add(k13);
            context.KekvCodes.Add(k14);
            context.KekvCodes.Add(k15);
            context.KekvCodes.Add(k16);
            context.SaveChanges();

            int priority = 1;
            MoneySource ms1 = new MoneySource { Name = "Місцевий бюджет", ViewPriority = priority++ };
            MoneySource ms2 = new MoneySource { Name = "Додаткова дотація", ViewPriority = priority++ };
            MoneySource ms3 = new MoneySource { Name = "Медична субвенція", ViewPriority = priority++ };
            MoneySource ms4 = new MoneySource { Name = "Долинська субвенція", ViewPriority = priority++ };
            MoneySource ms5 = new MoneySource { Name = "Біленьківська субвенція", ViewPriority = priority++ };
            MoneySource ms6 = new MoneySource { Name = "Широківська субвенція", ViewPriority = priority++ };
            MoneySource ms7 = new MoneySource { Name = "Суми за дорученням", ViewPriority = priority++ };
            MoneySource ms8 = new MoneySource { Name = "Спецрахунок", ViewPriority = priority++ };

            context.MoneySources.Add(ms1);
            context.MoneySources.Add(ms2);
            context.MoneySources.Add(ms3);
            context.MoneySources.Add(ms4);
            context.MoneySources.Add(ms5);
            context.MoneySources.Add(ms6);
            context.MoneySources.Add(ms7);
            context.MoneySources.Add(ms8);
            context.SaveChanges();

            base.Seed(context);
        }
    }
}


/****** Object:  StoredProcedure [dbo].[ISCustomerStatement]    Script Date: 2/25/2022 3:32:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE proc [dbo].[ISCustomerStatement]

@cardcode nvarchar(255),
@startdate date,
@enddate date

AS
Begin
SELECT T0.[DocDate] AS 'InvoicePostingDate' ,
T0.[DocNum] AS 'InvDocumentNumber',
T2.[CardCode] AS 'CustomerCode', T2.[CardName] AS 'CustomerName',
T0.[NumAtCard], T0.[DocTotal],T1.[SumApplied] AS 'PaidToInvoice', T2.[DocNum] AS 'PaymentDocumentNumber',
T2.[DocDate] AS 'PaymentPostingDate', T0.[DocTotal]- T1.[SumApplied] as 'RemainingPayment'
FROM [dbo].[OINV] T0 INNER JOIN [dbo].[RCT2] T1 ON T1.[DocEntry] = T0.[DocEntry]
INNER JOIN [dbo].[ORCT] T2 ON T2.[DocNum] = T1.[DocNum]
where t0.CardCode = @cardcode--'129564' 
and (t2.DocDate >= @startdate and t2.DocDate <= @enddate)

END
GO



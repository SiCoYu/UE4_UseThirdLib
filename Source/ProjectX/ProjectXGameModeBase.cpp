// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.


#include "ProjectXGameModeBase.h"
#include "SimpleOSSManage.h"
#include "Engine/Engine.h"

void Print(const FString &Msg, float Time = 10.f, FColor Color = FColor::Red)
{
	if (GEngine)
	{
		GEngine->AddOnScreenDebugMessage(-1, Time, Color, Msg);
	}
}

void AProjectXGameModeBase::BeginPlay()
{
	Super::BeginPlay();
	FString AccessKeyId = "123";
	FString AccessKeySecret = "456";
	FString EndPoint = ".aliyuncs.com";
	FString BucketName = "project";
	SIMPLE_OSS.InitAccounts(AccessKeyId, AccessKeySecret, EndPoint);
	bool is_exit = SIMPLE_OSS.DoesBucketExist(BucketName);
	if (is_exit)
	{
		Print("Exist Bucket", 99.0f);
	}
	else
	{
		Print("Don't Exist Bucket", 99.0f);
	}
}

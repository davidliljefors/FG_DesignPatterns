//using System;
//using System.Collections.Generic;

//public class Subscriber<T> : IObserver<T>, IDisposable
//{
//	private readonly IObserver<T> m_Observer;

//	public bool Unsubscribed { get; set; }

//	public Subscriber(IObserver<T> observer)
//	{
//		m_Observer = observer;
//	}

//	public void OnCompleted()
//	{
//		m_Observer.OnCompleted();
//	}

//	public void OnError(Exception error)
//	{
//		m_Observer.OnError(error);
//	}

//	public void OnNext(T value)
//	{
//		m_Observer.OnNext(value);
//	}

//	public void Dispose()
//	{
//		Unsubscribed = true;
//	}
//}

//public class Subject<T> : ISubject<T>
//{
//	private int m_Index = 0;

//	private readonly List<Subscriber<T>> m_Observers = new List<Subscriber<T>>();

//	public void OnCompleted()
//	{
//		for (m_Index = 0; m_Index < m_Observers.Count; m_Index++)
//		{
//			m_Observers[m_Index].OnCompleted();
//		}
//	}

//	public void OnError(Exception error)
//	{
//		for (m_Index = 0; m_Index < m_Observers.Count; m_Index++)
//		{
//			m_Observers[m_Index].OnCompleted();
//		}
//	}

//	public void OnNext(T value)
//	{
//		for (m_Index = 0; m_Index < m_Observers.Count; m_Index++)
//		{
//			m_Observers[m_Index].OnNext(value);
//		}
//	}

//	public IDisposable Subscribe(IObserver<T> observer)
//	{
//		m_Observers.Add(new Subscriber<T>(observer));
//		return new Subscription(this, observer);
//	}

//	private class Subscription : IDisposable
//	{
//		private readonly Subject<T> m_Subject;
//		private readonly IObserver<T> m_Observer;

//		public Subscription(Subject<T> subject, IObserver<T> observer)
//		{
//			m_Subject = subject;
//			m_Observer = observer;
//		}

//		public void Dispose()
//		{
//			int elementIndex = m_Subject.m_Observers.IndexOf(m_Observer);
//			if (elementIndex >= 0)
//			{
//				m_Subject.m_Observers.Remove(m_Observer);

//				if (elementIndex <= m_Subject.m_Index)
//				{
//					m_Subject.m_Index--;
//				}
//			}
//		}
//	}
//}